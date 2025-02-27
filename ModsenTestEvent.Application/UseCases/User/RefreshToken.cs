namespace ModsenTestEvent.Application.UseCases.User;

public class RefreshToken : IRefreshTokenUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshToken(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> ExecuteAsync(string refreshToken, CancellationToken cancellationToken)
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