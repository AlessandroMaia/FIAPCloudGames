namespace Application.Games.DTOs;

public sealed class GameSummaryDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required decimal BasePrice { get; init; }
    public int AgeRating { get; init; }
    public required string Genre { get; init; }
    public bool IsOnPromotion { get; set; }
    public decimal DiscountedPrice { get; init; }
    public decimal PercentageDiscount { get; init; } = 0m;
}
