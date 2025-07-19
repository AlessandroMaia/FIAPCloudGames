using Domain.GameLibraries;
using Domain.PromotionGames;
using Domain.Promotions;
using SharedKernel;

namespace Domain.Games;

public sealed class Game : Entity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal BasePrice { get; set; }
    public required int AgeRating { get; set; }
    public GameGenre Genre { get; set; }
    public bool IsActive { get; set; } = true;

    public List<PromotionGame> PromotionGames { get; } = [];
    public List<Promotion> Promotions { get; } = [];
    public List<GameLibrary> GameLibraries { get; set; } = [];
}
