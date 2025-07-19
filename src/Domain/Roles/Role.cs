namespace Domain.Roles;

public sealed class Role
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const int AdminId = 1;
    public const int UserId = 2;

    public static string[] All =>
        [Admin, User];

    public static Role[] AllComplete =>
        [
            new Role { Id = AdminId, Name = Admin },
            new Role { Id = UserId, Name = User }
        ];

    public int Id { get; init; }
    public required string Name { get; init; }
}
