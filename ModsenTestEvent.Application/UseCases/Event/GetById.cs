namespace ModsenTestEvent.Application.UseCases.Event;

public class GetById : IGetByIdUseCase<EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetById(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDto?> ExecuteAsync(int id)
    {
        var eventItem = await _eventRepository.GetAsync(id);
        if (eventItem == null) throw new NotFoundException("Event not found");

        return _mapper.Map<EventDto>(eventItem);
    }
}