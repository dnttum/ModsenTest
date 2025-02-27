namespace ModsenTestEvent.Application.UseCases.Event;

public class GetByName : IGetByNameUseCase<EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetByName(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDto> ExecuteAsync(string name)
    {
        var eventItem = await _eventRepository.GetAsync(name);
        if (eventItem == null) throw new NotFoundException("Event not found");

        return _mapper.Map<EventDto>(eventItem);
    }
}