using Application.Abstractions.Messaging;
using Application.Games.DTOs;
using Application.Libraries.Queries.GetUserLibrary;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Libraries;

public sealed record GetUserLibraryRequest([FromQuery] int? PageNumber = 1, [FromQuery] int PageSize = 10);

public sealed class GetUserLibrary : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{userId:guid}/libraries", async (
            [FromRoute] Guid userId,
            [AsParameters] GetUserLibraryRequest request,
            IQueryHandler<GetUserLibraryQuery, PagedResponse<GameMinimalDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserLibraryQuery(userId, request.PageNumber, request.PageSize);

            Result<PagedResponse<GameMinimalDto>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Libraries)
        .Produces<PagedResponse<GameMinimalDto>>(StatusCodes.Status200OK)
        .WithSummary("Get the list of games in a user's library.")
        .WithDescription("""
           Retrieves a paginated list of games associated with the user identified by `userId`.
        
           - The `userId` must be provided as a GUID in the route.
           - Returns a paged response containing game summaries.
           - Returns 200 OK if the data is retrieved successfully.
           - Returns an error response if the operation fails.
        
           **Available to all authenticated users.**
        """); ;
    }
}

