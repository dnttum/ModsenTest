namespace ModsenTestEvent.Infrastructure.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly DataContext _context;

    public ParticipantRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Participant> RegisterAsync(Participant participant)
    {
        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        return participant;
    }
    
    public async Task<IEnumerable<Participant?>> GetRangeAsync(int id)
    {
        var participants = await _context.Participants
            .Where(p => p.EventId == id)
            .ToListAsync();

        return participants;
    }

    public async Task<Participant?> GetAsync(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        
        return participant;
    }

    public async Task DeleteAsync(Participant participant)
    {
        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
    }
}