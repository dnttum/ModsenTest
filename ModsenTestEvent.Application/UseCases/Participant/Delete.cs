namespace ModsenTestEvent.Application.UseCases.Participant;

public class Delete : IDeleteUseCase
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ParticipantDto> _participantValidator;

    public Delete(IParticipantRepository participantRepository, IMapper mapper, IValidator<ParticipantDto> participantValidator)
    {
        _participantRepository = participantRepository;
        _mapper = mapper;
        _participantValidator = participantValidator;
    }
    public async Task ExecuteAsync(int id)
    {
        var participant = await _participantRepository.GetAsync(id);
        if (participant == null) throw new NotFoundException("Participant not found");
        
        await _participantRepository.DeleteAsync(participant);
    }
}