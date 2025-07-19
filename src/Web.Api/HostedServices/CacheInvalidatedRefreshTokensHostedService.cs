using Domain.RefreshTokens;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Api.HostedServices;

public class CacheInvalidatedRefreshTokensHostedservice(
    IMemoryCache cache,
    IServiceProvider service,
    ILogger<CacheInvalidatedRefreshTokensHostedservice> logger) 
        : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Initializing service for loading invalidated refresh tokens into the cache");

        using var scope = service.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var tokens = await context.RefreshTokens
            .Where(t => t.Invalidated)
            .ToListAsync(cancellationToken);

        int count = 0;
        foreach (var token in tokens)
        {
            cache.Set(token.JwtId, RevocatedTokenType.Invalidated);
            count++;
        }

        logger.LogInformation("{count} invalidated tokens stored in the cache", count);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Invalidated refresh token cache loading service stopping");
        return Task.CompletedTask;
    }
}
