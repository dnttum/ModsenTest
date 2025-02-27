namespace ModsenTestEvent.Application.UseCases.Event;

public class UploadImage : IUploadImageUseCase 
{
    private readonly IEventRepository _eventRepository;
    private readonly IFileService _fileService;
    private readonly IValidator<IFormFile> _fileValidator;

    public UploadImage(IEventRepository eventRepository, IFileService fileService, IValidator<IFormFile> fileValidator)
    {
        _eventRepository = eventRepository;
        _fileService = fileService;
        _fileValidator = fileValidator;
    }

    public async Task ExecuteAsync(int id, IFormFile file)
    {
        var eventItem = await _eventRepository.GetAsync(id);
        if (eventItem == null) throw new NotFoundException("Event not found");

        if (file == null || file.Length == 0) throw new FileRequiredException();

        await _fileValidator.ValidateAndThrowAsync(file);

        var filePath = await _fileService.SaveFileAsync(file);

        var image = new Image
        {
            Url = filePath,
            EventId = eventItem.Id
        };

        await _eventRepository.UploadImageAsync(image);
    }
}