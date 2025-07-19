using Domain.Users;
using System.Security.Claims;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    Task<(string token, string refreshToken)> Create(User user);
    Task<ClaimsPrincipal?> GetPrincipalFromToken(string token);
    string? GetEmail(ClaimsPrincipal principal);
    string? GetJti(ClaimsPrincipal principal);
    string? GetUserId(ClaimsPrincipal principal);
}
