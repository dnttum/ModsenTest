namespace ModsenTestEvent.Application.Interfaces;

public interface IUserService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken);
    Task<UserDto> RegisterAsync(RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
