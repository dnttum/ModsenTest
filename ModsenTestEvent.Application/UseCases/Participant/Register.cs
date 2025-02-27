namespace ModsenTestEvent.Application.UseCases.Participant;

public class Register : ICreateUseCase<ParticipantDto>
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ParticipantDto> _participantValidator;

    public Register(IParticipantRepository participantRepository, IMapper mapper, IValidator<ParticipantDto> participantValidator)
    {
        _participantRepository = participantRepository;
        _mapper = mapper;
        _participantValidator = participantValidator;
    }

    public async Task<ParticipantDto?> ExecuteAsync(ParticipantDto participantDto)
    {
        await _participantValidator.ValidateAndThrowAsync(participantDto);

        var participant = _mapper.Map<Domain.Models.Participant>(participantDto);
        var createdParticipant = await _participantRepository.RegisterAsync(participant);

        return _mapper.Map<ParticipantDto>(createdParticipant);
    }
}