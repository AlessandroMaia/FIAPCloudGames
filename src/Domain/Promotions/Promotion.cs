using Domain.Games;
using Domain.PromotionGames;
using SharedKernel;

namespace Domain.Promotions;

public sealed class Promotion : Entity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required decimal PercentageDiscount { get; set; }
    public required DateTime StartDateUtc { get; set; }
    public required DateTime EndDateUtc { get; set; }
    public bool IsActive { get; set; } = true;

    public List<PromotionGame> PromotionGames { get; } = [];
    public List<Game> Games { get; } = [];
}
