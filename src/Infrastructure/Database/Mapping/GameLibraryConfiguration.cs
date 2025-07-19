using Domain.GameLibraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping;

internal sealed class GameLibraryConfiguration : IEntityTypeConfiguration<GameLibrary>
{
    public void Configure(EntityTypeBuilder<GameLibrary> builder)
    {
        builder.HasKey(gl => new { gl.GameId, gl.UserId });

        builder.Property(gl => gl.GameId)
            .IsRequired();

        builder.Property(gl => gl.UserId)
            .IsRequired();

        builder.Property(gl => gl.AcquiredAtUtc)
            .IsRequired();

        builder.HasOne(gl => gl.User)
            .WithMany(u => u.PurchasedGames)
            .HasForeignKey(gl => gl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gl => gl.Game)
            .WithMany(g => g.GameLibraries)
            .HasForeignKey(gl => gl.GameId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
