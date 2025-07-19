using FluentValidation;

namespace Application.Users.Commands.ForceLogout;

internal sealed class ForceLogoutUserCommandValidator : AbstractValidator<ForceLogoutUserCommand>
{
    public ForceLogoutUserCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("UserId can't be empty");
    }
}
