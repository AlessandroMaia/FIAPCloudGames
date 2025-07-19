
using Application.Abstractions.Messaging;
using Application.Purchases.Commands.PurchaseGame;
using Domain.PurchaseHistories;
using Domain.Roles;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Purchases;

internal sealed class PurchaseGame : IEndpoint
{
    public sealed record Request(
        Guid GameId,
        Guid? PromotionId,
        PaymentMethod PaymentMethod);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("purchase", async (
            Request request,
            ICommandHandler<PurchaseGameCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new PurchaseGameCommand(
                request.GameId,
                request.PromotionId,
                request.PaymentMethod);

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Purchases)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Purchase a game.")
        .WithDescription("""
          Processes the purchase of a game using the specified payment method and optional promotion.
        
          - The request body must include the `GameId`, and may optionally include a `PromotionId` and a `PaymentMethod`.
          - Returns 204 No Content if the purchase is completed successfully.
          - Returns an error response if the purchase fails due to validation or business rules.
        
          **Requires the user to be authenticated.**
        """);
    }
}
