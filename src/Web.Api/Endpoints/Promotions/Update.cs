using Application.Abstractions.Messaging;
using Application.Promotions.Commands.Update;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Promotions;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string? Name,
        decimal? PercentageDiscount,
        DateTime? StartDate,
        DateTime? EndDate,
        Guid[] GameIds);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("promotions/{promotionId:guid}", async (
            [FromRoute] Guid promotionId,
            [FromBody] Request request,
            ICommandHandler<UpdatePromotionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdatePromotionCommand(
                promotionId,
                request.Name,
                request.PercentageDiscount,
                request.StartDate,
                request.EndDate,
                request.GameIds);

            Result result = await handler.Handle(command, cancellationToken);
            
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Promotions)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Update an existing promotion.")
        .WithDescription("""
           Updates the details of a specific promotion identified by `promotionId`.
           
           The request body can update:
           - `Name`: The promotion's name.
           - `PercentageDiscount`: The discount percentage.
           - `StartDate` / `EndDate`: The active period of the promotion.
           - `GameIds`: The list of games included in the promotion.
           
           Returns 204 No Content if the update is successful.
           
           **Requires administrator permissions.**
        """);
    }
}
