namespace ModsenTestEvent.Application.Validators;

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        RuleFor(file => file)
            .NotNull().WithMessage("File is required.")
            .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
            .Must(f => f.Length < 5 * 1024 * 1024).WithMessage("File must be less than 5MB.");
    }
}