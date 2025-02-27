namespace ModsenTestEvent.Application.Interfaces;

public interface ILoginUseCase
{
    Task<LoginResponseDto> ExecuteAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken);
}