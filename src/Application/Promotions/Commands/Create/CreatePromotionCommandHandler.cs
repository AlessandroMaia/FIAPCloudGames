using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Games;
using Domain.PromotionGames;
using Domain.Promotions;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Promotions.Commands.Create;

internal sealed class CreatePromotionCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreatePromotionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePromotionCommand command, CancellationToken cancellationToken)
    {
        if (await context.Promotions.AnyAsync(p => p.Name == command.Name && 
                                                   p.IsActive &&
                                                   p.StartDateUtc <= DateTime.UtcNow && 
                                                   p.EndDateUtc > DateTime.UtcNow, cancellationToken))
        {
            return Result.Failure<Guid>(PromotionErrors.NameNotUnique);
        }

        var promotion = new Promotion
        {
            Name = command.Name,
            PercentageDiscount = command.PercentageDiscount,
            StartDateUtc = command.StartDateUtc,
            EndDateUtc = command.EndDateUtc,
            IsActive = true,
        };

        await context.Promotions.AddAsync(promotion, cancellationToken);

        foreach (var gameId in command.GameIds)
        {
            var game = await context.Games.FindAsync([gameId], cancellationToken);

            if (game == null)
            {
                return Result.Failure<Guid>(GameErrors.NotFound(gameId));
            }

            await context.PromotionGames.AddAsync(new PromotionGame
            {
                GameId = gameId,
                PromotionId = promotion.Id,
            }, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        return promotion.Id;
    }
}
