using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Users.Commands.Logout;

internal sealed class LogoutUserCommandHandler(
    IApplicationDbContext context,
    ITokenProvider tokenProvider
    ) : ICommandHandler<LogoutUserCommand>
{
    public async Task<Result> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.Token) && string.IsNullOrEmpty(command.RefreshToken))
            return Result.Success();

        Guid userId = await GetUseIdFromTokenAsync(command, cancellationToken);

        if (userId == Guid.Empty)
            return Result.Success();

        await context.RefreshTokens
            .Where(r => r.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Guid> GetUseIdFromTokenAsync(LogoutUserCommand command, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(command.Token))
        {
            ClaimsPrincipal? principal = await tokenProvider.GetPrincipalFromToken(command.Token);
            if (principal is not null)
            {
                var userIdString = tokenProvider.GetUserId(principal);

                if (Guid.TryParse(userIdString, out Guid userId))
                    return userId;
            }
        }

        if (!string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            var refresh = await context.RefreshTokens
                .Where(r => r.Token == command.RefreshToken)
                .Select(r => new { r.UserId })
                .FirstOrDefaultAsync(cancellationToken);

            if (refresh is not null)
                return refresh.UserId;
        }

        return Guid.Empty;
    }
}
