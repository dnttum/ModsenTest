namespace ModsenTestEvent.Domain.Models;

public class Event : BaseModel
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public string? Location { get; set; }
    
    public string Category { get; set; }
    
    public int MaxCount { get; set; }
    
    public ICollection<Image> Images { get; set; }
    
    public ICollection<Participant> Participants { get; set; }
}