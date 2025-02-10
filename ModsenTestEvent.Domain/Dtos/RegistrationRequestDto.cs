namespace ModsenTestEvent.Domain.Dtos;

public class RegistrationRequestDto
{
    public string Name { get; set; }
    
    public string Contacts { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string Role { get; set; }
}