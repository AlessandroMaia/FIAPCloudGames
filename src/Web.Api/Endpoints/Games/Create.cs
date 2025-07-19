using Application.Abstractions.Messaging;
using Application.Games.Commands.Create;
using Domain.Games;
using Domain.Roles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Games;

internal sealed class Create : IEndpoint
{
    public sealed record Request(
        string Title, 
        string? Description, 
        decimal BasePrice, 
        int AgeRating, 
        GameGenre Genre);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("games", async (
            Request request,
            ICommandHandler<CreateGameCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateGameCommand(
                request.Title,
                request.Description,
                request.BasePrice,
                request.AgeRating,
                request.Genre);

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Games)
        .Produces<Guid>(StatusCodes.Status200OK)
        .WithSummary("Create a new game.")
        .WithDescription("""
           Creates a new game entry in the system with the provided details.
           
           - Accepts game information such as title, description, base price, age rating, and genre.
           - Returns 200 OK with the ID of the newly created game upon success.
           - Returns a problem response if validation fails or the operation cannot be completed.
           
           **Requires administrator permissions.**
        """);
    }
}
