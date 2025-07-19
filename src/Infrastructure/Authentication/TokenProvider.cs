using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.RefreshTokens;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(
    IConfiguration configuration, 
    IApplicationDbContext context, 
    TokenValidationParameters tokenValidationParameters)
        : ITokenProvider
{
    public async Task<(string token, string refreshToken)> Create(User user)
    {
        var token = await CreateToken(user);
        var refreshToken = await CreateRefreshToken(token, user);

        return (token, refreshToken);
    }

    public async Task<ClaimsPrincipal?> GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JsonWebTokenHandler();

        var tokenParameters = tokenValidationParameters.Clone();
        tokenParameters.ValidateLifetime = false; 

        var result = await tokenHandler.ValidateTokenAsync(token, tokenParameters);

        if (!result.IsValid)
        {
            return null;
        }

        if (result.SecurityToken is not JsonWebToken jwt)
        {
            return null;
        }

        if (!jwt.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return result.ClaimsIdentity != null ? new ClaimsPrincipal(result.ClaimsIdentity) : null;
    }

    public string? GetUserId(ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? 
            principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? GetEmail(ClaimsPrincipal principal)
        => principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

    public string? GetJti(ClaimsPrincipal principal)
        => principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

    private async Task<string> CreateToken(User user)
    {
        string secretKey = configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<string> roleNames = await context.UserRoles
            .Where(u => u.UserId == user.Id)
            .Select(u => u.Role.Name)
            .ToListAsync();

        var tokenId = Guid.NewGuid().ToString();
        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, tokenId),
        ];
        claims.AddRange(roleNames.Select(r => new Claim(ClaimTypes.Role, r)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    private async Task<string> CreateRefreshToken(string token, User user)
    {
        var principal = await GetPrincipalFromToken(token);
        var jti = principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            JwtId = jti!,
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
        };

        await context
            .RefreshTokens
            .Where(rt => rt.UserId == user.Id)
            .ExecuteDeleteAsync();

        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();

        return refreshToken.Token;
    }
}
