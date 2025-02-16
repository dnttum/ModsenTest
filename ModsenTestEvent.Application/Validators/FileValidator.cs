namespace ModsenTestEvent.Application.Validators;

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        RuleFor(file => file)
            .Must(f => f.Length < 5 * 1024 * 1024).WithMessage("File must be less than 5MB.");
    }
}