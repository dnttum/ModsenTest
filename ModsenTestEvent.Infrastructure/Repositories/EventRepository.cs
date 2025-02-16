namespace ModsenTestEvent.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly DataContext _context;

    public EventRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Event?>> GetAllAsync(PageParamsDto pageParamsDto, CancellationToken cancellationToken)
    {
        var events = await _context.Events
            .OrderBy(e => e.Id) 
            .ApplyPagination(pageParamsDto) 
            .ToListAsync(cancellationToken);
        
        return events;
    }

    public async Task<Event?> GetAsync(int id)
    { 
        var eventItem = await _context.Events.FindAsync(id);

        return eventItem;
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await _context.Events.AnyAsync(e => e.Name == name);
    }
    
    public async Task<Event?> GetAsync(string name) 
    {
        var eventItem = await _context.Events.FirstOrDefaultAsync(n => n.Name == name);

        return eventItem;
    }

    public async Task<Event?> CreateAsync(Event eventItem)
    {
        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();
        
        return eventItem;
    }

    public async Task UpdateAsync(Event eventItem) 
    {
        _context.Events.Update(eventItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Event eventItem)
    {
        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Event?>> GetAllAsync(DateTime? date, string? location, string? category) 
    {
        return await _context.Events.ToListAsync();
    }

    public async Task UploadImageAsync(Image image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
    }
}