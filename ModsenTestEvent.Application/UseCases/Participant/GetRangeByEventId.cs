namespace ModsenTestEvent.Application.UseCases.Participant;

public class GetRangeByEventId : IGetRangeByEventIdUseCase
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ParticipantDto> _participantValidator;

    public GetRangeByEventId(IParticipantRepository participantRepository, IMapper mapper, IValidator<ParticipantDto> participantValidator)
    {
        _participantRepository = participantRepository;
        _mapper = mapper;
        _participantValidator = participantValidator;
    }

    public async Task<IEnumerable<ParticipantDto>> ExecuteAsync(int eventId)
    {
        var participants = await _participantRepository.GetRangeAsync(eventId);
        
        return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
    }
}