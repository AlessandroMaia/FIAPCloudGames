using Application.Games.DTOs;
using Domain.Games;

namespace Application.Games.Mappings;

public static class GameMapping
{
    public static GameSummaryDto ToGameSummaryDto(this Game game)
    {
        var activePromotions = game.PromotionGames
            .Where(p => p.Promotion.IsActive &&
                        p.Promotion.StartDateUtc <= DateTime.UtcNow &&
                        p.Promotion.EndDateUtc >= DateTime.UtcNow)
            .OrderByDescending(p => p.Promotion.StartDateUtc)
            .ToList();

        return new GameSummaryDto
        {
            Id = game.Id,
            Title = game.Title,
            BasePrice = game.BasePrice,
            AgeRating = game.AgeRating,
            Genre = game.Genre.ToString(),
            IsOnPromotion = activePromotions.Count != 0,
            DiscountedPrice = activePromotions.Count != 0
                ? game.BasePrice * (1 - activePromotions.First().Promotion.PercentageDiscount / 100)
                : game.BasePrice,
            PercentageDiscount = activePromotions
                .Select(p => p.Promotion.PercentageDiscount)
                .FirstOrDefault()
        };
    }

    public static GameMinimalDto ToGameMinimalDto(this Game game)
    {
        return new GameMinimalDto
        {
            Id = game.Id,
            Title = game.Title,
            AgeRating = game.AgeRating,
            Genre = game.Genre.ToString(),
        };
    }
}