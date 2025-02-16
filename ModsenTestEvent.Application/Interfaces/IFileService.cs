namespace ModsenTestEvent.Application.Interfaces;

public interface IFileService
{
    public Task<string> SaveFileAsync(IFormFile file);
}