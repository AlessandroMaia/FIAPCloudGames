using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.GameLibraries;
using Domain.Games;
using Domain.PurchaseHistories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Purchases.Commands.PurchaseGame;

internal sealed class PurchaseGameCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext)
        : ICommandHandler<PurchaseGameCommand>
{
    public async Task<Result> Handle(PurchaseGameCommand command, CancellationToken cancellationToken)
    {
        if (userContext?.UserId is not Guid userId)
            return Result.Failure(UserErrors.Unauthorized());

        var gameAlreadyInLibrary = await context.GameLibraries
            .AnyAsync(gl => gl.UserId == userId && gl.GameId == command.GameId, cancellationToken);

        if (gameAlreadyInLibrary)
            return Result.Failure(GameErrors.AlreadyInLibrary(command.GameId));

        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure(UserErrors.NotFound(userId));

        var game = await context.Games
            .FirstOrDefaultAsync(g => g.Id == command.GameId, cancellationToken);

        if (game is null)
            return Result.Failure(GameErrors.NotFound(command.GameId));

        decimal finalPrice = game.BasePrice;

        if (command.PromotionId.HasValue)
        {
            var promotion = await context.Promotions
                .FirstOrDefaultAsync(p => p.Id == command.PromotionId.Value, cancellationToken);

            if (promotion is not null && promotion.IsActive && promotion.StartDateUtc <= DateTime.UtcNow && promotion.EndDateUtc >= DateTime.UtcNow)
            {
                finalPrice = promotion.PercentageDiscount > 0
                    ? Math.Round(game.BasePrice * (1 - promotion.PercentageDiscount / 100m), 2)
                    : game.BasePrice;
            }
        }

        var purchaseHistory = new PurchaseHistory
        {
            UserId = userId,
            GameId = game.Id,
            PaymentMethod = command.PaymentMethod,
            PricePaid = finalPrice,
        };

        await context.PurchaseHistories.AddAsync(purchaseHistory, cancellationToken);

        var gameLibrary = new GameLibrary
        {
            UserId = userId,
            GameId = game.Id,
            AcquiredAtUtc = DateTime.UtcNow
        };

        await context.GameLibraries.AddAsync(gameLibrary, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
