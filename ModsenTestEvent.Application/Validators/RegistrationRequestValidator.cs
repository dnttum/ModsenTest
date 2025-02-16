namespace ModsenTestEvent.Application.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequestDto>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
        
        RuleFor(x => x.Contacts)
            .NotEmpty().WithMessage("Contacts is required.")
            .Must(c => c.Contains("@")).WithMessage("Contacts must be a valid email address with @.");
        
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}