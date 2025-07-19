using FluentValidation;

namespace Application.Promotions.Commands.Update;

public class UpdatePromotionCommandValidator : AbstractValidator<UpdatePromotionCommand>
{
    public UpdatePromotionCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("promotion ID is required.");
    }
}
