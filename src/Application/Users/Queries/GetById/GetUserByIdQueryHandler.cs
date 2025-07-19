using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Application.Users.Mappings;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Queries.GetById;

internal sealed class GetUserByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetUserByIdQuery, UserDetailDto>
{
    public async Task<Result<UserDetailDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .SingleOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserDetailDto>(UserErrors.NotFound(query.UserId));
        }

        var roles = await context.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == query.UserId)
            .ToListAsync(cancellationToken);

        var refreshTokens = await context.RefreshTokens
            .Where(rt => rt.UserId == query.UserId)
            .ToListAsync(cancellationToken);

        var useDto = user.ToUserDetailDto(refreshTokens, roles);

        return useDto;
    }
}
