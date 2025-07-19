using Domain.Games;
using Domain.Promotions;

namespace Domain.PromotionGames;

public sealed class PromotionGame
{
    public required Guid GameId { get; set; }
    public required Guid PromotionId { get; set; }

    public Game Game { get; set; } = null!;
    public Promotion Promotion { get; set; } = null!;
}
