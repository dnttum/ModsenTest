namespace ModsenTestEvent.Infrastructure.Repositories;

[AutoInterface]
public class EventRepository : IEventRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public EventRepository(DataContext context, IMapper mapper, IEmailService emailService)
    {
        _context = context;
        _mapper = mapper;
        _emailService = emailService;
    }
    
    public async Task<IEnumerable<EventDto?>> GetAllAsync(PageParamsDto pageParamsDto, CancellationToken cancellationToken)
    {
        var events = await _context.Events
            .OrderBy(e => e.Id) 
            .ApplyPagination(pageParamsDto) 
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<EventDto>>(events);
    }

    public async Task<EventDto?> GetAsync(int id)
    { 
        var eventItem = await _context.Events.FindAsync(id);
        
        return _mapper.Map<EventDto>(eventItem);
    }

    public async Task<EventDto?> GetAsync(string name) 
    {
        var eventItem = await _context.Events.FirstOrDefaultAsync(n => n.Name == name);
        
        return _mapper.Map<EventDto>(eventItem);
    }

    public async Task<Event?> CreateAsync(EventDto eventDto)
    {
        var exist = await _context.Events.AnyAsync(e => e.Name == eventDto.Name);
        if (exist) throw new DuplicateEventException(eventDto.Name);
        
        var eventItem = _mapper.Map<Event>(eventDto);
        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();
        
        return eventItem;
    }

    public async Task UpdateAsync(int id, EventDto eventDto) 
    {
        var eventItem = await _context.Events.FindAsync(id);

        if (eventItem != null)
        {
            eventItem.Name = eventDto.Name;
            eventItem.Description = eventDto.Description;
            eventItem.DateTime = eventDto.DateTime;
            eventItem.Location = eventDto.Location;
            eventItem.Category = eventDto.Category;
            eventItem.Description = eventDto.Description;

            await _context.SaveChangesAsync();
            await _emailService.SendEmailAsync(id, eventItem);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem != null) 
        {
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EventDto> GetAsync(DateTime? date, string? location, string? category) 
    {
        var eventItem = await _context.Events.FindAsync(date, location, category);
        
        return _mapper.Map<EventDto>(eventItem);
    }

    public async Task UploadImageAsync(int id, IFormFile file)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null) throw new NotFoundException($"Event with ID {id} not found");

        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException("Upload directory not found");
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; 
        var filePath = Path.Combine(path, fileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        var image = new Image
        {
            Url = $"/uploads/{fileName}", 
            EventId = eventItem.Id
        };  
        
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }
}