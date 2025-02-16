namespace ModsenTestEvent.Application.Services;

public class TokenService : ITokenService
{
    private readonly string _secretKey;

    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:Key"]!;
    }

    public (string accessToken, string refreshToken) GenerateTokens(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = "ModsenTestEvent",
            Audience = "YourAudience",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return (accessToken, refreshToken);
    }
}