using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasMany(u => u.PurchasedGames)
            .WithOne(gl => gl.User)
            .HasForeignKey(gl => gl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.PurchaseHistory)
            .WithOne(ph => ph.User)
            .HasForeignKey(ph => ph.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
