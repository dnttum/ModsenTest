namespace ModsenTestEvent.Application.Interfaces;

public interface IEventService
{
    public Task<IEnumerable<EventDto?>> GetAllAsync(PageParamsDto pageParamsDto, CancellationToken cancellationToken);

    public Task<EventDto?> GetAsync(int id);

    public Task<EventDto> GetAsync(string name);

    public Task<EventDto?> CreateAsync(EventDto eventDto);

    public Task UpdateAsync(int id, EventDto eventDto);

    public Task DeleteAsync(int id);

    public Task<IEnumerable<EventDto>> GetAllAsync(DateTime? date, string? location, string? category);

    public Task UploadImageAsync(int id, IFormFile file);
}