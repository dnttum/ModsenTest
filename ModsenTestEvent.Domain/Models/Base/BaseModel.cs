namespace ModsenTestEvent.Domain.Models.Base;

public class BaseModel
{
    public int Id { get; set; }
    
    public DateTime CreatedOn { get; set;} = DateTime.UtcNow;        
}