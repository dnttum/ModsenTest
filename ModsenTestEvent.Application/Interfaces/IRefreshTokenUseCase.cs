namespace ModsenTestEvent.Application.Interfaces;

public interface IRefreshTokenUseCase
{
    Task<LoginResponseDto> ExecuteAsync(string refreshToken, CancellationToken cancellationToken);
}