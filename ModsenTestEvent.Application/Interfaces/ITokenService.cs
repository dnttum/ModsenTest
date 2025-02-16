namespace ModsenTestEvent.Application.Interfaces;

public interface ITokenService
{
    (string accessToken, string refreshToken) GenerateTokens(User user);
}