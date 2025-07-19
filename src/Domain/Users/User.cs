using Domain.GameLibraries;
using Domain.PurchaseHistories;
using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public required string PasswordHash { get; set; }

    public List<GameLibrary> PurchasedGames { get; set; } = [];
    public List<PurchaseHistory> PurchaseHistory { get; set; } = [];
}