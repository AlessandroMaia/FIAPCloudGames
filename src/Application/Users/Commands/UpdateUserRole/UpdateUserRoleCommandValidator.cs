using FluentValidation;

namespace Application.Users.Commands.UpdateUserRole;

internal sealed class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(c => c.NewRole)
            .NotEmpty()
            .WithMessage("Role cannot be empty.");

        RuleFor(c => c.UserId)
           .NotEmpty()
           .WithMessage("UserId cannot be empty.");
    }
}
