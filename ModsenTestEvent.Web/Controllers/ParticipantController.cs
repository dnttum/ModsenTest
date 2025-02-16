namespace ModsenTestEvent.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "ParticipantPolicy")]
public class ParticipantController : ControllerBase
{
    private readonly IParticipantService _participantService;

    public ParticipantController(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(ParticipantDto participantDto)
    {
        var participant = await _participantService.RegisterAsync(participantDto);
        
        return Ok(participant);
    }

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetRangeAsync(int eventId)
    {
        var participants = await _participantService.GetRangeAsync(eventId);
        
        return Ok(participants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var participant = await _participantService.GetAsync(id);
        
        return Ok(participant);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _participantService.DeleteAsync(id);
        
        return NoContent();
    }
}