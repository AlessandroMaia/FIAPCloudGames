using Application.Abstractions.Messaging;
using Application.Promotions.DTOs;
using Application.Promotions.Queries.Get;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Promotions;

public sealed record GetRequest(
    [FromQuery] string? Name,
    [FromQuery] decimal? InitialPercentageDiscount,
    [FromQuery] decimal? FinalPercentageDiscount,
    [FromQuery] DateTime? StartDate,
    [FromQuery] DateTime? EndDate,
    [FromQuery] bool? IsActive,
    [FromQuery] int? PageNumber = 1,
    [FromQuery] int PageSize = 10);
internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("promotions", async (
            [AsParameters] GetRequest request,
            IQueryHandler<GetPromotionsQuery, PagedResponse<PromotionSummaryDto>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPromotionsQuery(
                request.Name,
                request.InitialPercentageDiscount,
                request.FinalPercentageDiscount,
                request.StartDate,
                request.EndDate,
                request.IsActive,
                request.PageNumber,
                request.PageSize);

            Result<PagedResponse<PromotionSummaryDto>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Promotions)
        .Produces<PagedResponse<PromotionSummaryDto>>(StatusCodes.Status200OK)
        .WithSummary("Retrieve a paginated list of promotions with optional filters.")
        .WithDescription("""
           Returns a paginated list of promotions, optionally filtered by:
           
           - `Name`
           - Percentage discount range (`InitialPercentageDiscount`, `FinalPercentageDiscount`)
           - Date range (`StartDate`, `EndDate`)
           - Activity status (`IsActive`)
           
           Supports pagination using `PageNumber` and `PageSize` query parameters.
           
           **Available to all authenticated users.**
        """);
    }
}

