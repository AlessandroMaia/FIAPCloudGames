using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Games.DTOs;
using Application.Games.Mappings;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Games.Queries.Get;

internal sealed class GetGamesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetGamesQuery, PagedResponse<GameSummaryDto>>
{
    public async Task<Result<PagedResponse<GameSummaryDto>>> Handle(GetGamesQuery query, CancellationToken cancellationToken)
    {
        var games = context.Games
            .Include(g => g.PromotionGames)
                .ThenInclude(pg => pg.Promotion)
            .Where(g => g.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Title))
            games = games.Where(g => g.Title.Contains(query.Title!));

        if (query.InitialBasePrice.HasValue)
            games = games.Where(g => g.BasePrice >= query.InitialBasePrice.Value);

        if (query.FinalBasePrice.HasValue)
            games = games.Where(g => g.BasePrice <= query.FinalBasePrice.Value);

        if (query.AgeRating.HasValue)
            games = games.Where(g => g.AgeRating == query.AgeRating.Value);

        if (!string.IsNullOrWhiteSpace(query.Genre))
            games = games.Where(g => g.Genre.ToString() == query.Genre!);

        if (query.IsOnPromotion.HasValue && query.IsOnPromotion.Value)
            games = games.Where(g => g.PromotionGames.Any(pg => pg.Promotion.IsActive &&
                                                                pg.Promotion.StartDateUtc <= DateTime.UtcNow &&
                                                                pg.Promotion.EndDateUtc >= DateTime.UtcNow));

        var total = await games.CountAsync(cancellationToken);

        var filtredGames = await games
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip((query.PageNumber!.Value - 1) * query.PageSize!)
            .Take(query.PageSize)
            .Select(g => g.ToGameSummaryDto())
            .ToListAsync(cancellationToken);

        var result = new PagedResponse<GameSummaryDto>(filtredGames, query.PageNumber.Value, query.PageSize, total);

        return result;
    }
}
