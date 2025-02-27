namespace ModsenTestEvent.Application.UseCases.Event;

public class Create : ICreateUseCase<EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<EventDto> _eventValidator;

    public Create(IEventRepository eventRepository, IMapper mapper, IValidator<EventDto> eventValidator)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _eventValidator = eventValidator;
    }

    public async Task<EventDto?> ExecuteAsync(EventDto eventDto)
    {
        await _eventValidator.ValidateAndThrowAsync(eventDto);

        if (await _eventRepository.ExistsAsync(eventDto.Name))
        {
            throw new DuplicateEventException(eventDto.Name);
        }

        var eventItem = _mapper.Map<Domain.Models.Event>(eventDto);
        var createdEvent = await _eventRepository.CreateAsync(eventItem);

        return _mapper.Map<EventDto>(createdEvent);
    }
}