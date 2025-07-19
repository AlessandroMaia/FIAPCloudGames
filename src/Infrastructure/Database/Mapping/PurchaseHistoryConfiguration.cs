using Domain.PurchaseHistories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping;

internal sealed class PurchaseHistoryConfiguration : IEntityTypeConfiguration<PurchaseHistory>
{
    public void Configure(EntityTypeBuilder<PurchaseHistory> builder)
    {
        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.UserId)
            .IsRequired();

        builder.Property(ph => ph.GameId)
            .IsRequired();

        builder.Property(ph => ph.PricePaid)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(g => g.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.IsRefunded)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(ph => ph.User)
            .WithMany(u => u.PurchaseHistory)
            .HasForeignKey(ph => ph.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ph => ph.Game)
            .WithMany()
            .HasForeignKey(ph => ph.GameId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
