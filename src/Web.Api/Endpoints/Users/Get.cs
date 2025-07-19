using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Application.Users.Queries.Get;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

public sealed record GetRequest([FromQuery] int? PageNumber = 1, [FromQuery] int PageSize = 10);
internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async (
            [AsParameters] GetRequest request,
            IQueryHandler<GetUsersQuery, PagedResponse<UserSummaryDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUsersQuery(request.PageNumber, request.PageSize);

            Result<PagedResponse<UserSummaryDto>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Users)
        .Produces<PagedResponse<UserSummaryDto>>(StatusCodes.Status200OK)
        .WithSummary("Retrieve a paginated list of users.")
        .WithDescription("""
           Returns a paginated list of users.
           
           - Supports pagination via `PageNumber` and `PageSize` query parameters.
           - Only accessible by administrators.
           
           Returns 200 OK with paged user summaries or an error response.
        """);
    }
}