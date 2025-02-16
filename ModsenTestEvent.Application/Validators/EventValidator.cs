namespace ModsenTestEvent.Application.Validators;

public class EventValidator : AbstractValidator<EventDto>
{
    public EventValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(100).WithMessage("Event name must be less than 100 characters.");

        RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(e => e.DateTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Event date must be in the future.");

        RuleFor(e => e.Location)
            .NotEmpty().WithMessage("Location is required.");

        RuleFor(e => e.Category)
            .NotEmpty().WithMessage("Category is required.");
    }
}