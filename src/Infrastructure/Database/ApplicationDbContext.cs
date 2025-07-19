using Application.Abstractions.Data;
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

namespace Infrastructure.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionGame> PromotionGames { get; set; }
    public DbSet<GameLibrary> GameLibraries { get; set; }
    public DbSet<PurchaseHistory> PurchaseHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}
