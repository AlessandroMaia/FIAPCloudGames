using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Promotions.DTOs;
using Application.Promotions.Mappings;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Promotions.Queries.Get;

internal sealed class GetPromotionsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetPromotionsQuery, PagedResponse<PromotionSummaryDto>>
{
    public async Task<Result<PagedResponse<PromotionSummaryDto>>> Handle(GetPromotionsQuery query, CancellationToken cancellationToken)
    {
        var promotions = context.Promotions
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            promotions = promotions.Where(p => p.Name.Contains(query.Name!));

        if (query.InitialPercentageDiscount.HasValue)
            promotions = promotions.Where(p => p.PercentageDiscount >= query.InitialPercentageDiscount.Value);

        if (query.FinalPercentageDiscount.HasValue)
            promotions = promotions.Where(p => p.PercentageDiscount <= query.FinalPercentageDiscount.Value);

        if (query.StartDateUtc.HasValue)
            promotions = promotions.Where(p => p.StartDateUtc >= query.StartDateUtc.Value);

        if (query.EndDateUtc.HasValue)
            promotions = promotions.Where(p => p.EndDateUtc <= query.EndDateUtc.Value);

        if (query.IsActive.HasValue)
            promotions = promotions.Where(p => p.IsActive == query.IsActive.Value);

        var total = await promotions.CountAsync(cancellationToken);

        var filteredPromotions = await promotions
            .OrderByDescending(p => p.CreatedAtUtc)
            .Skip((query.PageNumber!.Value - 1) * query.PageSize!)
            .Take(query.PageSize)
            .Select(p => p.ToPromotionSummaryDto())
            .ToListAsync(cancellationToken);

        return new PagedResponse<PromotionSummaryDto>(filteredPromotions, query.PageNumber.Value, query.PageSize, total);
    }
}
