using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasData(
            new Role { Id = Role.AdminId, Name = Role.Admin },
            new Role { Id = Role.UserId, Name = Role.User }
        );
    }
}
