using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Domain.RefreshTokens;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Commands.Refresh;

internal sealed class RefreshUserCommandHandler(
    IApplicationDbContext context,
    ITokenProvider tokenProvider) : ICommandHandler<RefreshUserCommand, CredentialsDto>
{
    public async Task<Result<CredentialsDto>> Handle(RefreshUserCommand command, CancellationToken cancellationToken)
    {
        RefreshToken? refreshToken = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == command.RefreshToken, cancellationToken);

        if(refreshToken is null || refreshToken.Invalidated || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            return Result.Failure<CredentialsDto>(UserErrors.RefreshTokenInvalid);
        }

        var user = refreshToken.User;
        if (user is null)
        {
            return Result.Failure<CredentialsDto>(UserErrors.RefreshTokenInvalid);
        }

        var (newToken, newRefreshToken) = await tokenProvider.Create(user);

        return new CredentialsDto(newToken, newRefreshToken);
    }
}
