namespace ModsenTestEvent.Domain.Models;

public class Image : BaseModel
{
    public required string Url { get; set; }
    
    public Event? Event { get; set; }
    
    public int EventId { get; set; }
}