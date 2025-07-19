using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Commands.ForceLogout;

internal sealed class ForceLogoutUserCommandHandler(IApplicationDbContext context)
    : ICommandHandler<ForceLogoutUserCommand>
{
    public async Task<Result> Handle(ForceLogoutUserCommand command, CancellationToken cancellationToken)
    {
        await context.RefreshTokens
            .Where(r => r.UserId == command.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
