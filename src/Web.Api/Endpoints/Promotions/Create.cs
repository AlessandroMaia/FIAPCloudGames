using Application.Abstractions.Messaging;
using Application.Promotions.Commands.Create;
using Domain.Roles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Promotions;

internal sealed class Create : IEndpoint
{
    public sealed record Request(
        string Name,
        decimal PercentageDiscount,
        DateTime StartDate,
        DateTime EndDate,
        Guid[] GameIds);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("promotions", async (
            Request request,
            ICommandHandler<CreatePromotionCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreatePromotionCommand(
                request.Name,
                request.PercentageDiscount,
                request.StartDate,
                request.EndDate,
                request.GameIds);

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Promotions)
        .Produces<Guid>(StatusCodes.Status200OK)
        .WithSummary("Create a new promotion for one or more games.")
        .WithDescription("""
           Creates a new promotion that applies a percentage discount to one or more games during a specified date range.
           
           The request body must include:
           - `Name`: The name of the promotion.
           - `PercentageDiscount`: The discount percentage (e.g., 10 for 10%).
           - `StartDate` and `EndDate`: The promotion's active period in .
           - `GameIds`: A list of game IDs to which the promotion applies.
           
           Returns 200 OK with the ID of the created promotion if successful.
           
           **Requires administrator permissions.**
        """);
    }
}
