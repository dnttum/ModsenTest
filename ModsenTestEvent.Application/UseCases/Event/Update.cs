namespace ModsenTestEvent.Application.UseCases.Event;

public class Update : IUpdateUseCase<EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<EventDto> _eventValidator;
    private readonly IEmailService _emailService;

    public Update(IEventRepository eventRepository, IMapper mapper, IValidator<EventDto> eventValidator, IEmailService emailService)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _eventValidator = eventValidator;
        _emailService = emailService;
    }

    public async Task ExecuteAsync(int id, EventDto eventDto)
    {
        var existingEvent = await _eventRepository.GetAsync(id);
        if (existingEvent == null) throw new NotFoundException("Event not found");

        await _eventValidator.ValidateAndThrowAsync(eventDto);

        _mapper.Map(eventDto, existingEvent);

        await _eventRepository.UpdateAsync(existingEvent);
        await _emailService.SendEmailAsync(id, existingEvent);
    }
}