namespace ModsenTestEvent.Application.Interfaces;

public interface IUploadImageUseCase
{
    Task ExecuteAsync(int id, IFormFile file);
}