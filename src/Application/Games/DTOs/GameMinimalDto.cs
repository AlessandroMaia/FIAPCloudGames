namespace Application.Games.DTOs;

public sealed class GameMinimalDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public int AgeRating { get; init; }
    public required string Genre { get; init; }
}
