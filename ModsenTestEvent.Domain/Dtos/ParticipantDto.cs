namespace ModsenTestEvent.Domain.Dtos;

public class ParticipantDto 
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    
    public string Email { get; set; }
    
    public int? EventId { get; set; }
}