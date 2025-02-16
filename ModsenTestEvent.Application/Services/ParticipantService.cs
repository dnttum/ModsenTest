namespace ModsenTestEvent.Application.Services;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IValidator<ParticipantDto> _participantValidator;
    private readonly IMapper _mapper;

    public ParticipantService(IParticipantRepository participantRepository, IMapper mapper, IValidator<ParticipantDto> participantValidator)
    {
        _participantRepository = participantRepository;
        _mapper = mapper;
        _participantValidator = participantValidator;
    }

    public async Task<ParticipantDto?> RegisterAsync(ParticipantDto participantDto)
    {
        await _participantValidator.ValidateAndThrowAsync(participantDto);
        
        var participant = _mapper.Map<Participant>(participantDto);
        
        var participantItem = await _participantRepository.RegisterAsync(participant);
        
        return _mapper.Map<ParticipantDto>(participantItem);
    }

    public async Task<IEnumerable<ParticipantDto?>> GetRangeAsync(int id)
    {
        var participants = await _participantRepository.GetRangeAsync(id);
        
        return _mapper.Map<IEnumerable<ParticipantDto?>>(participants);
    }

    public async Task<ParticipantDto?> GetAsync(int id)
    {
        var participant = await _participantRepository.GetAsync(id);
        if (participant == null) throw new NotFoundException("Participant not found");
        
        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task DeleteAsync(int id)
    {
        var participant = await _participantRepository.GetAsync(id);
        if (participant == null) throw new NotFoundException("Participant not found");
        
        await _participantRepository.DeleteAsync(participant);
    }
    
}