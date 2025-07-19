using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Games;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Promotions.Commands.Delete;

internal sealed class DeletePromotionCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeletePromotionCommand>
{
    public async Task<Result> Handle(DeletePromotionCommand command, CancellationToken cancellationToken)
    {
        var promotion = await context.Promotions
            .SingleOrDefaultAsync(g => g.Id == command.PromotionId, cancellationToken);

        if (promotion is null)
            return Result.Failure(GameErrors.NotFound(command.PromotionId));

        promotion.IsActive = false;
        promotion.UpdatedAtUtc = DateTime.UtcNow;

        context.Promotions.Update(promotion);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
