using Domain.Games;
using Domain.Users;
using SharedKernel;

namespace Domain.PurchaseHistories;

public sealed class PurchaseHistory : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public required decimal PricePaid { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public bool IsRefunded { get; set; } = false;

    public User User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
