namespace ModsenTestEvent.Application.UseCases.User;

public class Login : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public Login(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
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
}