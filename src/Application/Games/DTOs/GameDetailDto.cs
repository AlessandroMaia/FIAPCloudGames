using Domain.Games;

namespace Application.Games.DTOs;

public sealed class GameDetailDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal BasePrice { get; set; }
    public required int AgeRating { get; set; }
    public GameGenre Genre { get; set; }
    public bool IsActive { get; set; } = true;
}
