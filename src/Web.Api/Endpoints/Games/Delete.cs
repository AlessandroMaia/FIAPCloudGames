using Application.Abstractions.Messaging;
using Application.Games.Commands.Delete;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Games;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("games/{gameId:guid}", async (
            [FromRoute] Guid gameId,
            ICommandHandler<DeleteGameCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteGameCommand(gameId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Games)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Delete a game by ID.")
        .WithDescription("""
           Deletes a specific game from the system by its unique identifier (`gameId`).
           
           - Requires the game ID to be passed as a route parameter.
           - Returns 204 No Content if the game is successfully deleted.
           - Returns an error response if the game is not found or the operation fails.
           
           **Requires administrator permissions.**
        """);
    }
}
