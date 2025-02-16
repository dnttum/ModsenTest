namespace ModsenTestEvent.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PageParamsDto pageParamsDto, CancellationToken cancellationToken)
    {
        var events = await _eventService.GetAllAsync(pageParamsDto, cancellationToken);
        
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var eventItem = await _eventService.GetAsync(id);
        
        return Ok(eventItem);
    }

    [HttpGet("byname/{name}")] 
    public async Task<IActionResult> GetAsync(string name)
    {
        var eventItem = await _eventService.GetAsync(name);
        
        return Ok(eventItem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(EventDto eventDto)
    {
        var eventItem = await _eventService.CreateAsync(eventDto);
        
        return Ok(eventItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, EventDto eventDto)
    {
        await _eventService.UpdateAsync(id, eventDto);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEventAsync(int id)
    {
        await _eventService.DeleteAsync(id);
        
        return NoContent();
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetAsync([FromQuery] DateTime? date, [FromQuery] string? location, [FromQuery] string? category)
    {
        var events = await _eventService.GetAllAsync(date, location, category);
        return Ok(events);
    }

    [HttpPost("files/{eventId}")] 
    public async Task<IActionResult> UploadImageAsync(int eventId, IFormFile? file)
    {
        await _eventService.UploadImageAsync(eventId, file);
        
        return NoContent();
    }
}