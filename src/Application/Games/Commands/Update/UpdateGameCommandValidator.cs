using FluentValidation;

namespace Application.Games.Commands.Update;

public class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
{
    public UpdateGameCommandValidator()
    {
        RuleFor(command => command.GameId)
            .NotEmpty().WithMessage("Game ID is required.");
    }
}
