using Application.Users.DTOs;
using Domain.RefreshTokens;
using Domain.UserRoles;
using Domain.Users;

namespace Application.Users.Mappings;

public static class UserMapping
{
    public static UserSummaryDto ToUserSummaryDto(this User user, List<RefreshToken> RefreshTokens)
    {
        return new UserSummaryDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            IsOnline = RefreshTokens
                    .Any(r => r.UserId == user.Id && !r.Invalidated && r.ExpiresOnUtc >= DateTime.UtcNow)
        };
    }

    public static UserDetailDto ToUserDetailDto(this User user, List<RefreshToken> RefreshTokens, List<UserRole> Roles)
    {
        return new UserDetailDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAtUtc = user.CreatedAtUtc,
            UpdatedAtUtc = user.UpdatedAtUtc,
            IsOnline = RefreshTokens
                    .Any(r => r.UserId == user.Id && !r.Invalidated && r.ExpiresOnUtc >= DateTime.UtcNow),
            Roles = [.. Roles.Select(r => r.Role.Name)]
        };
    }
}
