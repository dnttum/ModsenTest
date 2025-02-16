namespace ModsenTestEvent.Application.Interfaces;

public interface IParticipantService
{
    public Task<ParticipantDto?> RegisterAsync(ParticipantDto participantDto);
    
    public Task<IEnumerable<ParticipantDto?>> GetRangeAsync(int eventId);
    
    public Task <ParticipantDto?> GetAsync(int id);
    
    public Task DeleteAsync(int id);
}