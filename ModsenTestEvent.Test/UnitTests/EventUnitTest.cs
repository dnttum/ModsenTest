namespace ModsenTestEvent.Test.UnitTests;

[TestFixture]
public class EventUnitTest
{
    private DataContext _context;
    private IMapper _mapper;
    private Mock<IEmailService> _emailServiceMock;
    private EventRepository _repository;

    private DataContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new DataContext(options);
    }

    [SetUp]
    public void Setup()
    {
        _context = GetInMemoryDbContext();

        var config = new MapperConfiguration(cfg => { cfg.CreateMap<Event, EventDto>().ReverseMap(); });
        _mapper = new Mapper(config);

        _emailServiceMock = new Mock<IEmailService>();

        _repository = new EventRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnCorrectEventCount()
    {
        var faker = new Faker<Event>()
            .RuleFor(e => e.Name, f => f.Lorem.Sentence())
            .RuleFor(e => e.Description, f => f.Lorem.Paragraph())
            .RuleFor(e => e.DateTime, f => f.Date.Future())
            .RuleFor(e => e.Location, f => f.Address.City())
            .RuleFor(e => e.Category, f => f.Commerce.Department())
            .RuleFor(e => e.MaxCount, f => f.Random.Int(10, 200));

        var testEvents = faker.Generate(2);  
        await _context.Events.AddRangeAsync(testEvents);
        await _context.SaveChangesAsync();

        var pageParams = new PageParamsDto { Page = 1, PageSize = 10 };
        
        var events = await _repository.GetAllAsync(pageParams, CancellationToken.None);
        
        events.Should().HaveCount(2, "Должно быть 2 события в базе данных.");
    }

    [Test]
    public async Task CreateAsync_ShouldSaveEventToDatabase()
    {
        var faker = new Faker<Event>()
            .RuleFor(e => e.Name, f => f.Lorem.Sentence())
            .RuleFor(e => e.Description, f => f.Lorem.Paragraph())
            .RuleFor(e => e.DateTime, f => f.Date.Future())
            .RuleFor(e => e.Location, f => f.Address.City())
            .RuleFor(e => e.Category, f => f.Commerce.Department())
            .RuleFor(e => e.MaxCount, f => f.Random.Int(10, 200));

        var eventItem = faker.Generate();
        
        var createdEvent = await _repository.CreateAsync(eventItem);
        var events = await _context.Events.ToListAsync();
        
        events.Should().HaveCount(1, "Должно быть 1 событие в базе данных.");
        events.First().Should().Match<Event>(e =>
                e.Name == eventItem.Name &&
                e.Description == eventItem.Description &&
                e.Location == eventItem.Location &&
                e.Category == eventItem.Category,
            "Событие должно быть сохранено с правильными данными.");
    }
}
