namespace ModsenTestEvent.Application.UseCases.Participant;

public class GetById : IGetByIdUseCase<ParticipantDto>
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ParticipantDto> _participantValidator;

    public GetById(IParticipantRepository participantRepository, IMapper mapper, IValidator<ParticipantDto> participantValidator)
    {
        _participantRepository = participantRepository;
        _mapper = mapper;
        _participantValidator = participantValidator;
    }

    public async Task<ParticipantDto?> ExecuteAsync(int id)
    {
        var participant = await _participantRepository.GetAsync(id);
        if (participant == null) throw new NotFoundException("Participant not found");
        
        return _mapper.Map<ParticipantDto>(participant);
    }
}