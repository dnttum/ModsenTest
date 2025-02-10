namespace ModsenTestEvent.Infrastructure.Repositories;
[AutoInterface]

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly string? secretKey;

    public UserRepository(DataContext context, IConfiguration configuration)
    {
        _context = context;
        secretKey = configuration.GetSection("Jwt")["Key"];
    }

    public async Task<bool> IsUserUniqueAsync(string username, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken: cancellationToken);
        if (user == null) return true;
        
        return false;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.UserName.ToLower() == loginRequestDto.Username.ToLower() && x.Password == loginRequestDto.Password, cancellationToken: cancellationToken);
        if (user == null)
        {
            return new LoginResponseDto()
            {
                Token = string.Empty,
                User = null
            };
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

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
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 

        await _context.SaveChangesAsync(cancellationToken);
        
        var loginResponseDto = new LoginResponseDto
        {
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken,
            User = user
        };
        return loginResponseDto;
    }

    public async Task<User> RegisterAsync(RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken)
    {
        var isUserUnique = await IsUserUniqueAsync(registrationRequestDto.UserName, cancellationToken);
        if (!isUserUnique)
        {
            throw new DuplicateUserException("Username is already taken.");
        }
        
        var user = new User
        {
            Name = registrationRequestDto.Name,
            Contacts = registrationRequestDto.Contacts,
            UserName = registrationRequestDto.UserName,
            Password = registrationRequestDto.Password,
            Role = registrationRequestDto.Role
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        user.Password = string.Empty;
        return user;
    }
    
    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);
    
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new SecurityTokenException("Invalid refresh token");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

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
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var newAccessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            User = user
        };
    }
}