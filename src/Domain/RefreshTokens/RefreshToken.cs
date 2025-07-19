using Domain.Users;

namespace Domain.RefreshTokens;

public sealed class RefreshToken
{
    public string Token { get; set; } = null!;

    public string JwtId { get; set; } = null!;

    public DateTime ExpiresOnUtc { get; set; }

    public bool Invalidated { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}
