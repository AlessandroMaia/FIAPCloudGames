using FluentValidation;

namespace Application.Promotions.Commands.Create;

public sealed class CreatePromotionCommandValidator
    : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Promotion name is required.")
            .MaximumLength(100)
            .WithMessage("Promotion name must not exceed 100 characters.");

        RuleFor(x => x.PercentageDiscount)
            .InclusiveBetween(0, 100)
            .WithMessage("Discount percentage must be between 0 and 100.");

        RuleFor(x => x.StartDateUtc)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .LessThanOrEqualTo(x => x.EndDateUtc)
            .WithMessage("Start date must be before or equal to end date.")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Start date must be greater or equal to today.");

        RuleFor(x => x.EndDateUtc)
            .NotEmpty()
            .WithMessage("End date is required.");
    }
}
