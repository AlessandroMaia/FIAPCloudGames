using FluentValidation;

namespace Application.Promotions.Commands.Delete;

internal sealed class DeletePromotionCommandValidator : AbstractValidator<DeletePromotionCommand>
{
    public DeletePromotionCommandValidator()
    {
        RuleFor(x => x.PromotionId)
            .NotEmpty()
            .WithMessage("Promotion ID must not be empty.");
    }
}
