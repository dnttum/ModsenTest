namespace ModsenTestEvent.Domain.Dtos;

public class EventDto 
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public string Location { get; set; }
    
    public string Category { get; set; }
    
    public int MaxCount { get; set; }
    
    public string? ImageUrl { get; set; }
}