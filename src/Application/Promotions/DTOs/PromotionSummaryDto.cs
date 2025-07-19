namespace Application.Promotions.DTOs;

public class PromotionSummaryDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required decimal PercentageDiscount { get; init; }
    public required DateTime StartDateUtc { get; init; }
    public required DateTime EndDateUtc { get; init; }
    public required bool IsActive { get; init; }
}
