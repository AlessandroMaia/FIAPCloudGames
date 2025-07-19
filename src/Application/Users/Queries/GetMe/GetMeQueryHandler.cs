using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Application.Users.Mappings;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Queries.GetMe;

internal sealed class GetMeQueryHandler(IApplicationDbContext context, IUserContext userContext)
    : IQueryHandler<GetMeQuery, UserDetailDto>
{
    public async Task<Result<UserDetailDto>> Handle(GetMeQuery query, CancellationToken cancellationToken)
    {
        if (userContext?.UserId is null)
        {
            return Result.Failure<UserDetailDto>(UserErrors.Unauthorized());
        }

        var user = await context.Users
            .SingleOrDefaultAsync(u => u.Id == userContext.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserDetailDto>(UserErrors.NotFound(userContext.UserId));
        }

        var roles = await context.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userContext.UserId)
            .ToListAsync(cancellationToken);

        var refreshTokens = await context.RefreshTokens
            .Where(rt => rt.UserId == userContext.UserId)
            .ToListAsync(cancellationToken);

        var useDto = user.ToUserDetailDto(refreshTokens, roles);

        return useDto;
    }
}
