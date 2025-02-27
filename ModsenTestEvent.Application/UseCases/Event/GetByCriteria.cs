namespace ModsenTestEvent.Application.UseCases.Event;

public class GetByCriteria : IGetByCriteriaUseCase<EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetByCriteria(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EventDto>> ExecuteAsync(DateTime? date, string? location, string? category)
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
}