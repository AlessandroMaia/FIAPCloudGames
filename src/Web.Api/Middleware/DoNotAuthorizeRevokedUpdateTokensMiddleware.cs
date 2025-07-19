using Domain.RefreshTokens;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Api.Middleware;

public class DoNotAuthorizeRevokedUpdateTokensMiddleware(
    RequestDelegate next,
    IMemoryCache cache)
{
    public Task Invoke(HttpContext context)
    {
        var skipPaths = new[]
        {
            "/users/login",
            "/users/refresh",
            "/users/register",
            "/users/logout"
        };
        if (skipPaths.Contains(context.Request.Path.Value, StringComparer.OrdinalIgnoreCase))
        {
            return next.Invoke(context);
        }

        var jwtId = context.User.FindFirst(JwtRegisteredClaimNames.Jti);
        var role = context.User.FindFirst(ClaimTypes.Role);
        if (jwtId is null || role is null)
        {
            return next.Invoke(context);
        }

        var revocationType = cache.Get<RevocatedTokenType?>(jwtId.Value);
        if (revocationType.HasValue)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return next.Invoke(context);
        }

        return next.Invoke(context);
    }
}
