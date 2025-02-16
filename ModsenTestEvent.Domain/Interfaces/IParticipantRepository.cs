namespace ModsenTestEvent.Domain.Interfaces;

public interface IParticipantRepository
{
    public Task<Participant> RegisterAsync(Participant participant);
    
    public Task<IEnumerable<Participant?>> GetRangeAsync(int id);

    public Task<Participant?> GetAsync(int id);
    
    public Task DeleteAsync(Participant participant);
}