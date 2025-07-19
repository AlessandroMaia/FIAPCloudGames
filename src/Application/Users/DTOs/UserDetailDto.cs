namespace Application.Users.DTOs;

public sealed class UserDetailDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public bool IsOnline { get; set; } = false;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public string[] Roles { get; set; } = [];
}
