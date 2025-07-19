using Application.Abstractions.Messaging;
using Application.Promotions.Commands.Delete;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Promotions;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("promotions/{promotionId:guid}", async (
            [FromRoute] Guid promotionId,
            ICommandHandler<DeletePromotionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeletePromotionCommand(promotionId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Promotions)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Delete a promotion by ID.")
        .WithDescription("""
           Deletes a promotion identified by its unique `promotionId`.
           
           - Accepts the promotion ID as a route parameter.
           - Returns 204 No Content if the promotion was successfully deleted.
           - Returns an error response if the promotion does not exist or deletion fails.
           
           **Requires administrator permissions.**
        """);
    }
}
