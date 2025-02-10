namespace ModsenTestEvent.Domain.Models;

public class Participant : BaseModel
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    
    public string Email { get; set; }
    
    public int EventId { get; set; }
    
    public Event Event { get; set; }
}