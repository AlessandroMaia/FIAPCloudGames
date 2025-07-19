using Domain.Games;
using Domain.Users;

namespace Domain.GameLibraries;

public sealed class GameLibrary
{
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public DateTime AcquiredAtUtc { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
