using Domain.Roles;
using Domain.Users;

namespace Domain.UserRoles;

public sealed class UserRole
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }

    public User User { get; } = null!;
    public Role Role { get; } = null!;
}
