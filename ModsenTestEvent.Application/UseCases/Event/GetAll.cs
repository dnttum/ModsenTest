namespace ModsenTestEvent.Application.UseCases.Event;

public class GetAll : IGetAllUseCase<EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetAll(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventDto>> ExecuteAsync(PageParamsDto pageParamsDto, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(pageParamsDto, cancellationToken);
        return _mapper.Map<IEnumerable<EventDto>>(events);
    }
}
