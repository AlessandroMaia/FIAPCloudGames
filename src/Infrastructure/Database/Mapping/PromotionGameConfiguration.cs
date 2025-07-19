using Domain.PromotionGames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping;

internal sealed class PromotionGameConfiguration : IEntityTypeConfiguration<PromotionGame>
{
    public void Configure(EntityTypeBuilder<PromotionGame> builder)
    {
        builder.HasKey(pg => new { pg.GameId, pg.PromotionId });

        builder.Property(pg => pg.GameId)
            .IsRequired();

        builder.Property(pg => pg.PromotionId)
            .IsRequired();

        builder.HasOne(pg => pg.Game)
            .WithMany(g => g.PromotionGames)
            .HasForeignKey(pg => pg.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pg => pg.Promotion)
            .WithMany(p => p.PromotionGames)
            .HasForeignKey(pg => pg.PromotionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
