using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Application.Users.Mappings;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Queries.Get;

internal sealed class GetUsersQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetUsersQuery, PagedResponse<UserSummaryDto>>
{
    public async Task<Result<PagedResponse<UserSummaryDto>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var total = await context.Users.CountAsync(cancellationToken);

        var users = await context.Users
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip((query.PageNumber!.Value - 1) * query.PageSize!)
            .Take(query.PageSize)
            .Select(u => u.ToUserSummaryDto(context.RefreshTokens.ToList()))
            .ToListAsync(cancellationToken);

        var result = new PagedResponse<UserSummaryDto>(users, query.PageNumber.Value, query.PageSize, total);

        return result;
    }
}