namespace ModsenTestEvent.Application.Validators;

public class ParticipantValidator : AbstractValidator<ParticipantDto>
{
    public ParticipantValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(p => p.DateOfBirth)
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(p => p.EventId)
            .GreaterThan(0).WithMessage("Invalid event ID");
    }
}