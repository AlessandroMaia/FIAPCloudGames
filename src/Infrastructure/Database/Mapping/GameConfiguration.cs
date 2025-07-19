using Domain.Games;
using Domain.PromotionGames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;

namespace Infrastructure.Database.Mapping;

internal sealed class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(g => g.BasePrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(g => g.Genre)
            .HasConversion<string>()
            .IsRequired();

        builder.HasIndex(g => g.Title)
            .IsUnique();

        builder.HasMany(g => g.Promotions)
            .WithMany(p => p.Games)
            .UsingEntity<PromotionGame>();

        builder.HasMany(g => g.GameLibraries)
            .WithOne(gl => gl.Game)
            .HasForeignKey(gl => gl.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(g => g.HasCheckConstraint("CK_Game_Genre", $"Genre IN ({ConstraintEnumExtension.GetEnumValuesForCheckConstraint<GameGenre>()})"));
    }
}
