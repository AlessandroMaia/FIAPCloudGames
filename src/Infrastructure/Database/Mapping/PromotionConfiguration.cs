using Domain.PromotionGames;
using Domain.Promotions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping;

internal sealed class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.PercentageDiscount)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(p => p.StartDateUtc)
            .IsRequired();

        builder.Property(p => p.EndDateUtc)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasMany(g => g.Games)
           .WithMany(p => p.Promotions)
           .UsingEntity<PromotionGame>();
    }
}
