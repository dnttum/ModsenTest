namespace ModsenTestEvent.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IGetAllUseCase<EventDto> _getAll;
    private readonly IGetByIdUseCase<EventDto> _getById;
    private readonly IGetByNameUseCase<EventDto> _getByName;
    private readonly ICreateUseCase<EventDto> _create;
    private readonly IUpdateUseCase<EventDto> _update;
    private readonly IDeleteUseCase _delete;
    private readonly IGetByCriteriaUseCase<EventDto> _getByCriteria;
    private readonly IUploadImageUseCase _uploadImage;

    public EventController(
        IGetAllUseCase<EventDto> getAll,
        IGetByIdUseCase<EventDto> getById,
        IGetByNameUseCase<EventDto> getByName,
        ICreateUseCase<EventDto> create,
        IUpdateUseCase<EventDto> update,
        IDeleteUseCase delete,
        IUploadImageUseCase uploadImage, IGetByCriteriaUseCase<EventDto> getByCriteria)
    {
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _create = create;
        _update = update;
        _delete = delete;
        _uploadImage = uploadImage;
        _getByCriteria = getByCriteria;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PageParamsDto pageParamsDto, CancellationToken cancellationToken)
    {
        var events = await _getAll.ExecuteAsync(pageParamsDto, cancellationToken);
        
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var eventItem = await _getById.ExecuteAsync(id);
        
        return Ok(eventItem);
    }

    [HttpGet("byname/{name}")]
    public async Task<IActionResult> GetAsync(string name)
    {
        var eventItem = await _getByName.ExecuteAsync(name);
        
        return Ok(eventItem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(EventDto eventDto)
    {
        var eventItem = await _create.ExecuteAsync(eventDto);
        
        return Ok(eventItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, EventDto eventDto)
    {
        await _update.ExecuteAsync(id, eventDto);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEventAsync(int id)
    {
        await _delete.ExecuteAsync(id);
        
        return NoContent();
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetAsync([FromQuery] DateTime? date, [FromQuery] string? location, [FromQuery] string? category)
    {
        var events = await _getByCriteria.ExecuteAsync(date, location, category);
        return Ok(events);
    }
    
    [HttpPost("files/{eventId}")]
    public async Task<IActionResult> UploadImageAsync(int eventId, IFormFile? file)
    {
        await _uploadImage.ExecuteAsync(eventId, file);
        
        return NoContent();
    }
}
