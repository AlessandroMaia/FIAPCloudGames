using FluentValidation;

namespace Application.Games.Commands.Create;

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(command => command.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(command => command.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than zero.");

        RuleFor(command => command.AgeRating)
            .InclusiveBetween(0, 18).WithMessage("Age rating must be between 0 and 18.");

        RuleFor(command => command.Genre)
            .NotEmpty().WithMessage("Genre is required.");
    }
}
