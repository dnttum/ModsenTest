namespace ModsenTestEvent.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "ParticipantPolicy")]
public class ParticipantController : ControllerBase
{
    private readonly ICreateUseCase<ParticipantDto> _createUseCase;
    private readonly IGetRangeByEventIdUseCase _getRangeUseCase;
    private readonly IGetByIdUseCase<ParticipantDto> _getByIdUseCase;
    private readonly IDeleteUseCase _deleteUseCase;

    public ParticipantController(
        IGetRangeByEventIdUseCase getRangeUseCase,
        IGetByIdUseCase<ParticipantDto> getByIdUseCase,
        IDeleteUseCase deleteUseCase, ICreateUseCase<ParticipantDto> createUseCase)
    {
        _getRangeUseCase = getRangeUseCase;
        _getByIdUseCase = getByIdUseCase;
        _deleteUseCase = deleteUseCase;
        _createUseCase = createUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(ParticipantDto participantDto)
    {
        var participant = await _createUseCase.ExecuteAsync(participantDto);
         
        return Ok(participant);
    }

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetRangeAsync(int eventId)
    {
        var participants = await _getRangeUseCase.ExecuteAsync(eventId);
        
        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var participant = await _getByIdUseCase.ExecuteAsync(id);
        
        return Ok(participant);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _deleteUseCase.ExecuteAsync(id);
        
        return NoContent();
    }
}