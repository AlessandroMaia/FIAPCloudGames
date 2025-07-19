using FluentValidation;

namespace Application.Games.Commands.Delete;

internal sealed class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
{
    public DeleteGameCommandValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("Game ID must not be empty.");
    }
}
