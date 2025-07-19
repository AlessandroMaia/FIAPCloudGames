using Application.Abstractions.Messaging;
using Application.Games.Commands.Update;
using Domain.Games;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Games;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string? Title, 
        string? Description, 
        decimal? BasePrice, 
        int? AgeRating, 
        GameGenre? Genre);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("games/{gameId:guid}", async (
            [FromRoute] Guid gameId,
            Request request,
            ICommandHandler<UpdateGameCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateGameCommand(
                gameId,
                request.Title,
                request.Description,
                request.BasePrice,
                request.AgeRating,
                request.Genre);

            Result result = await handler.Handle(command, cancellationToken);
            
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Games)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Update an existing game.")
        .WithDescription("""
           Updates the details of an existing game identified by `gameId`.
           
           - Accepts updated game data in the request body.
           - Returns 204 No Content if the update is successful.
           - Returns an error response if the game does not exist or validation fails.
           
           **Requires administrator permissions.**
        """);
    }
}
