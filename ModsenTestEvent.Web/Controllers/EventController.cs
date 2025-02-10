using ModsenTestEvent.Domain.Models;

namespace ModsenTestEvent.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly IValidator<IFormFile> _validator;

    public EventController(IEventRepository eventRepository, IValidator<IFormFile> validator)
    {
        _eventRepository = eventRepository;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PageParamsDto pageParamsDto, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(pageParamsDto, cancellationToken);
        
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var eventItem = await _eventRepository.GetAsync(id);
        if (eventItem == null) return NotFound();
        
        return Ok(eventItem);
    }

    [HttpGet("byname/{name}")] 
    public async Task<IActionResult> GetAsync(string name)
    {
        var eventItem = await _eventRepository.GetAsync(name);
        if (eventItem == null) return NotFound();
        
        return Ok(eventItem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(EventDto eventDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var eventItem = await _eventRepository.CreateAsync(eventDto);
        
        return Ok(eventItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, EventDto eventDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existingEvent = await _eventRepository.GetAsync(id);
        if (existingEvent == null) return NotFound();
        
        await _eventRepository.UpdateAsync(id, eventDto);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEventAsync(int id)
    {
        var eventItem = await _eventRepository.GetAsync(id);
        if (eventItem == null) return NotFound();
        
        await _eventRepository.DeleteAsync(id);
        
        return NoContent();
    }

    [HttpPost("files/{eventId}")] 
    public async Task<IActionResult> UploadImageAsync(int eventId, IFormFile? file)
    {
        var validationResult = await _validator.ValidateAsync(file);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }
        
        await _eventRepository.UploadImageAsync(eventId, file);
        
        return NoContent();
    }
}