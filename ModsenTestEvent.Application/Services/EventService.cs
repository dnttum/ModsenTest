namespace ModsenTestEvent.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IEmailService _emailService;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper; 
    private readonly IValidator<EventDto> _eventValidator;
    private readonly IValidator<IFormFile> _fileValidator;

    public EventService(IEventRepository eventRepository, IEmailService emailService, IFileService fileService, IMapper mapper, IValidator<IFormFile> fileValidator, IValidator<EventDto> eventValidator)
    {
        _eventRepository = eventRepository;
        _emailService = emailService;
        _fileService = fileService;
        _mapper = mapper;
        _eventValidator = eventValidator;
        _fileValidator = fileValidator;
    }

    public async Task<IEnumerable<EventDto?>> GetAllAsync(PageParamsDto pageParamsDto, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(pageParamsDto, cancellationToken);
        
        return _mapper.Map<IEnumerable<EventDto>>(events);
    }

    public async Task<EventDto?> GetAsync(int id)
    {
        var eventItem = await _eventRepository.GetAsync(id);

        if (eventItem == null) throw new NotFoundException("Event not found"); 
        
        return _mapper.Map<EventDto>(eventItem);
    }

    public async Task<EventDto> GetAsync(string name)
    {
        var eventItem = await _eventRepository.GetAsync(name);
        
        if (eventItem == null)
        {
            throw new NotFoundException($"Event not found");
        }
        
        return _mapper.Map<EventDto>(eventItem);
    }
    
    public async Task<EventDto?> CreateAsync(EventDto eventDto)
    {
        await _eventValidator.ValidateAndThrowAsync(eventDto);
        
        if (await _eventRepository.ExistsAsync(eventDto.Name))
        {
            throw new DuplicateEventException(eventDto.Name);
        }
        
        var eventItem = _mapper.Map<Event>(eventDto);
        var eventResult = await _eventRepository.CreateAsync(eventItem);

        return _mapper.Map<EventDto>(eventResult);
    }

    public async Task UpdateAsync(int id, EventDto eventDto)
    {
        var eventSearch = await _eventRepository.GetAsync(id);
        if (eventSearch == null) 
        {
            throw new NotFoundException("Event not found");
        }
        
        await _eventValidator.ValidateAndThrowAsync(eventDto);
        
        _mapper.Map(eventDto, eventSearch);

        await _eventRepository.UpdateAsync(eventSearch);
        await _emailService.SendEmailAsync(id, eventSearch);
    }

    public async Task DeleteAsync(int id)
    {
        var eventSearch = await _eventRepository.GetAsync(id);
        if (eventSearch == null) 
        {
            throw new NotFoundException("Event not found");
        }

        await _eventRepository.DeleteAsync(eventSearch); 
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync(DateTime? date, string? location, string? category)
    {
        var events = await _eventRepository.GetAllAsync(date, location, category);
        
        var filteredEvents = events
            .Where(e => 
                (!date.HasValue || e.DateTime.Date == date.Value.Date) &&
                (string.IsNullOrEmpty(location) || e.Location.Contains(location)) &&
                (string.IsNullOrEmpty(category) || e.Category.Contains(category)))
            .ToList();
        
        if (!filteredEvents.Any()) throw new NotFoundException("No events found matching the criteria");
        
        return _mapper.Map<IEnumerable<EventDto>>(filteredEvents);
    }
    
    public async Task UploadImageAsync(int id, IFormFile file)
    {
        var eventItem = await _eventRepository.GetAsync(id);
        if (eventItem == null) throw new NotFoundException("Event not found");
        
        if (file == null) 
            throw new FileRequiredException(); 

        if (file.Length == 0) 
            throw new EmptyFileException();
        
        await _fileValidator.ValidateAndThrowAsync(file);
        
        var filePath = await _fileService.SaveFileAsync(file);
        
        var image = new Image
        {
            Url = filePath,
            EventId = eventItem.Id
        };

        await _eventRepository.UploadImageAsync(image);
    }
}