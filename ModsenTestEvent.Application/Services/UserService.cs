namespace ModsenTestEvent.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IValidator<RegistrationRequestDto> _registrationRequestValidator;

    public UserService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper, IValidator<RegistrationRequestDto> registrationRequestValidator)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _registrationRequestValidator = registrationRequestValidator;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(loginRequestDto.Username, cancellationToken);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var (accessToken, refreshToken) = _tokenService.GenerateTokens(user);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            User = user
        };
    }

    public async Task<UserDto> RegisterAsync(RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken)
    {
        await _registrationRequestValidator.ValidateAndThrowAsync(registrationRequestDto, cancellationToken);
        
        if (!await _userRepository.IsUsernameUniqueAsync(registrationRequestDto.UserName, cancellationToken))
        {
            throw new DuplicateUserException("Username is already taken");
        }

        if (!await _userRepository.IsContactsUniqueAsync(registrationRequestDto.Contacts, cancellationToken))
        {
            throw new DuplicateEventException("Contacts is already taken");
        }
        
        var user = _mapper.Map<User>(registrationRequestDto);
        user.Password = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password);

        await _userRepository.AddAsync(user, cancellationToken);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new InvalidRefreshTokenException("Invalid refresh token");
        }

        var (newAccessToken, newRefreshToken) = _tokenService.GenerateTokens(user);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            User = user
        };
    }
}
