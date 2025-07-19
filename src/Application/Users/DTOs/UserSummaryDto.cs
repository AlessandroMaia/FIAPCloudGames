namespace Application.Users.DTOs;

public sealed class UserSummaryDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public bool IsOnline { get; set; } = false;
}

