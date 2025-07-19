using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.PromotionGames;
using Domain.Promotions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Promotions.Commands.Update;

internal sealed class UpdatePromotionCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdatePromotionCommand>
{
    public async Task<Result> Handle(UpdatePromotionCommand command, CancellationToken cancellationToken)
    {
        var promotion = await context.Promotions
            .Include(p => p.PromotionGames)
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (promotion is null)
            return Result.Failure(PromotionErrors.NotFound(command.Id));

        if (command.Name is not null)
            promotion.Name = command.Name;

        if (command.PercentageDiscount is not null)
            promotion.PercentageDiscount = command.PercentageDiscount.Value;

        if (command.StartDateUtc is not null)
            promotion.StartDateUtc = command.StartDateUtc.Value;

        if (command.EndDateUtc is not null)
            promotion.EndDateUtc = command.EndDateUtc.Value;

        promotion.IsActive = true;
        promotion.UpdatedAtUtc = DateTime.UtcNow;

        context.Promotions.Update(promotion);

        var newGameIdsSet = command.GameIds
            .ToHashSet();

        var existingGameIdsSet = promotion.PromotionGames
            .Select(pg => pg.GameId)
            .ToHashSet();

        var gamesToAdd = newGameIdsSet
            .Except(existingGameIdsSet)
            .Select(id => new PromotionGame
            {
                PromotionId = promotion.Id,
                GameId = id
            });

        var gamesToRemove = promotion.PromotionGames
            .Where(pg => !newGameIdsSet.Contains(pg.GameId))
            .ToList();

        if (gamesToRemove.Any())
            context.PromotionGames.RemoveRange(gamesToRemove);

        if (gamesToAdd.Any())
            await context.PromotionGames.AddRangeAsync(gamesToAdd, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
