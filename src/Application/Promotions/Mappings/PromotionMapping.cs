using Application.Promotions.DTOs;
using Domain.Promotions;

namespace Application.Promotions.Mappings;

public static class PromotionMapping
{
    public static PromotionSummaryDto ToPromotionSummaryDto(this Promotion promotion)
    {
        return new PromotionSummaryDto
        {
            Id = promotion.Id,
            Name = promotion.Name,
            PercentageDiscount = promotion.PercentageDiscount,
            StartDateUtc = promotion.StartDateUtc,
            EndDateUtc = promotion.EndDateUtc,
            IsActive = promotion.IsActive,
        };
    }
}
