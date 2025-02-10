namespace ModsenTestEvent.Infrastructure.Repositories;

[AutoInterface]
public class ParticipantRepository : IParticipantRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ParticipantRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ParticipantDto> RegisterAsync(ParticipantDto participantDto)
    {
        var participant = _mapper.Map<Participant>(participantDto);
        
        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<ParticipantDto>(participant);
    }
    
    public async Task<IEnumerable<ParticipantDto?>> GetRangeAsync(int id)
    {
        var participants = await _context.Participants
            .Where(p => p.EventId == id)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<ParticipantDto?>>(participants);
    }

    public async Task<ParticipantDto?> GetAsync(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        
        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task DeleteAsync(int id)
    {
        var participant = await _context.Participants.FindAsync(id);

        if (participant != null)
        {
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }
    }
}