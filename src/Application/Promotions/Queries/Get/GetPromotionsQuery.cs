using Application.Abstractions.Messaging;
using Application.Promotions.DTOs;
using SharedKernel;

namespace Application.Promotions.Queries.Get;

public sealed record GetPromotionsQuery(
    string? Name,
    decimal? InitialPercentageDiscount,
    decimal? FinalPercentageDiscount,
    DateTime? StartDateUtc,
    DateTime? EndDateUtc,
    bool? IsActive,
    int? PageNumber = 1,
    int PageSize = 10)
        : IQuery<PagedResponse<PromotionSummaryDto>>;
