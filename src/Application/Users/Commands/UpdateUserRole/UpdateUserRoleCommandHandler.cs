using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.RefreshTokens;
using Domain.Roles;
using Domain.UserRoles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SharedKernel;

namespace Application.Users.Commands.UpdateUserRole;

internal sealed class UpdateUserRoleCommandHandler(IApplicationDbContext context, IMemoryCache cache)
    : ICommandHandler<UpdateUserRoleCommand>
{
    public async Task<Result> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name.Trim().ToLower() == command.NewRole.Trim().ToLower(), cancellationToken);
        if (role == null)
        {
            return Result.Failure(RolesErrors.NotFound(command.NewRole));
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        await context.UserRoles
            .Where(r => r.UserId == command.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        var userRole = new UserRole
        {
            RoleId = role.Id,
            UserId = user.Id
        };

        await context.UserRoles.AddAsync(userRole, cancellationToken);

        var refreshTokens = await context.RefreshTokens
            .Where(rt => rt.UserId == user.Id && !rt.Invalidated)
            .ToListAsync(cancellationToken);

        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.Invalidated = true;

            cache.Set(refreshToken.JwtId, RevocatedTokenType.RoleChanged);
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
