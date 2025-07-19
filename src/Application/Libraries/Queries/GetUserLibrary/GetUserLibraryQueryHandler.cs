using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Games.DTOs;
using Application.Games.Mappings;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Libraries.Queries.GetUserLibrary;

internal sealed class GetUserLibraryQueryHandler(IApplicationDbContext context, IUserContext userContext)
    : IQueryHandler<GetUserLibraryQuery, PagedResponse<GameMinimalDto>>
{
    public async Task<Result<PagedResponse<GameMinimalDto>>> Handle(GetUserLibraryQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<PagedResponse<GameMinimalDto>>(UserErrors.NotFound(query.UserId));
        }

        if (userContext is not { IsAdmin: true } && userContext.UserId != user.Id)
        {
            return Result.Failure<PagedResponse<GameMinimalDto>>(UserErrors.Unauthorized());
        }

        var gameLibraries = context.GameLibraries
            .AsNoTracking()
            .Where(gl => gl.UserId == query.UserId)
            .OrderByDescending(gl => gl.AcquiredAtUtc)
            .Select(gl => gl.Game)
            .AsQueryable();

        var total = await gameLibraries.CountAsync(cancellationToken);

        var games = await gameLibraries
            .Skip((query.PageNumber!.Value - 1) * query.PageSize!)
            .Take(query.PageSize)
            .Select(g => g.ToGameMinimalDto())
            .ToListAsync(cancellationToken);

        var result = new PagedResponse<GameMinimalDto>(games, query.PageNumber.Value, query.PageSize, total);

        return result;
    }
}

