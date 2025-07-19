using Application.Abstractions.Messaging;
using Application.Games.DTOs;
using Application.Games.Queries.Get;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Games;

public sealed record GetRequest(
    [FromQuery] string? Title,
    [FromQuery] decimal? InitialBasePrice,
    [FromQuery] decimal? FinalBasePrice,
    [FromQuery] int? AgeRating,
    [FromQuery] string? Genre,
    [FromQuery] bool? IsOnPromotion,
    [FromQuery] int? PageNumber = 1,
    [FromQuery] int PageSize = 10);
internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("games", async (
            [AsParameters] GetRequest request,
            IQueryHandler<GetGamesQuery, PagedResponse<GameSummaryDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetGamesQuery(
                request.Title,
                request.InitialBasePrice,
                request.FinalBasePrice,
                request.AgeRating,
                request.Genre,
                request.IsOnPromotion,
                request.PageNumber,
                request.PageSize);

            Result<PagedResponse<GameSummaryDto>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Games)
        .Produces<PagedResponse<GameSummaryDto>>(StatusCodes.Status200OK)
        .WithSummary("Retrieve a paginated list of games with optional filters.")
        .WithDescription("""
           Returns a paginated list of games, optionally filtered by:
           
           - Title
           - Base price range
           - Age rating
           - Genre
           - Promotion status
           
           Supports pagination through `pageNumber` and `pageSize` query parameters.
           
           **Available to all authenticated users.**
        """);
    }
}

