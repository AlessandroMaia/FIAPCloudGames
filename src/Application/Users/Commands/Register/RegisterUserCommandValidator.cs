using FluentValidation;

namespace Application.Users.Commands.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("The name cannot be empty");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("The email cannot be empty")
            .EmailAddress()
            .WithMessage("The email is not valid");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("The password cannot be empty")
            .MinimumLength(8)
            .WithMessage("Password must contain at least 8 characters")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).+$")
            .WithMessage("The password must contain letters, numbers and special characters");
    }
}
