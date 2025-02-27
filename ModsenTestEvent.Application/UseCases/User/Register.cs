namespace ModsenTestEvent.Application.UseCases.User;

public class Register : IRegisterUseCase<RegistrationRequestDto, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<RegistrationRequestDto> _registrationRequestValidator;

    public Register(IUserRepository userRepository, IMapper mapper, IValidator<RegistrationRequestDto> registrationRequestValidator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _registrationRequestValidator = registrationRequestValidator;
    }

    public async Task<UserDto> ExecuteAsync(RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken)
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

        var user = _mapper.Map<Domain.Models.User>(registrationRequestDto);
        user.Password = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password);

        await _userRepository.AddAsync(user, cancellationToken);
        return _mapper.Map<UserDto>(user);
    }
}