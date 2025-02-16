using ModsenTestEvent.Domain.Dtos;

namespace ModsenTestEvent.Domain.Interfaces;

public interface IEventRepository
{
    public Task<IEnumerable<Event?>> GetAllAsync(PageParamsDto pageParamsDto, CancellationToken cancellationToken);

    public Task<Event?> GetAsync(int id);
    
    public Task<bool> ExistsAsync(string name);

    public Task<Event?> GetAsync(string name);

    public Task<Event?> CreateAsync(Event eventItem);

    public Task UpdateAsync(Event eventItem);

    public Task DeleteAsync(Event eventItem);

    public Task<IEnumerable<Event?>> GetAllAsync(DateTime? date, string? location, string? category);

    public Task UploadImageAsync(Image image);
}