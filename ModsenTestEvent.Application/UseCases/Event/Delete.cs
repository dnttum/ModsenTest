namespace ModsenTestEvent.Application.UseCases.Event;

public class Delete : IDeleteUseCase
{
    private readonly IEventRepository _eventRepository;

    public Delete(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task ExecuteAsync(int id)
    {
        var eventItem = await _eventRepository.GetAsync(id);
        if (eventItem == null) throw new NotFoundException("Event not found");

        await _eventRepository.DeleteAsync(eventItem);
    }
}