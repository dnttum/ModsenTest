namespace ModsenTestEvent.Application.Services;

public class FileService : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file)
    {
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

        return $"/uploads/{fileName}";
    }
}