namespace ModsenTestEvent.Domain.Dtos;

public class LoginResponseDto
{
    public User User { get; set; }
    
    public string Token { get; set; }
    
    public string? RefreshToken { get; set; }
}