namespace ModsenTestEvent.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "ParticipantPolicy")]
public class ParticipantController : ControllerBase
{
    private readonly IParticipantRepository _repository;

    public ParticipantController(IParticipantRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(ParticipantDto participantDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var participant = await _repository.RegisterAsync(participantDto);
        
        return Ok(participant);
    }

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetRangeAsync(int eventId)
    {
        var participants = await _repository.GetRangeAsync(eventId);
        
        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var participant = await _repository.GetAsync(id);
        if (participant == null) return NotFound();
        
        return Ok(participant);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var participant = await _repository.GetAsync(id);
        if (participant == null) return NotFound();
        
        await _repository.DeleteAsync(id);
        
        return NoContent();
    }
}