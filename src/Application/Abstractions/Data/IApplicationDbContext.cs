using Domain.GameLibraries;
using Domain.Games;
using Domain.PromotionGames;
using Domain.Promotions;
using Domain.PurchaseHistories;
using Domain.RefreshTokens;
using Domain.Roles;
using Domain.UserRoles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<Role> Roles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Game> Games { get; }
    DbSet<Promotion> Promotions { get; }
    DbSet<PromotionGame> PromotionGames { get; }
    DbSet<GameLibrary> GameLibraries { get; }
    DbSet<PurchaseHistory> PurchaseHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
