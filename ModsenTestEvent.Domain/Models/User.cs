namespace ModsenTestEvent.Domain.Models;

public class User : BaseModel
{
    public string Name { get; set; }
    
    public string Contacts { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string Role { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryTime { get; set; }
}