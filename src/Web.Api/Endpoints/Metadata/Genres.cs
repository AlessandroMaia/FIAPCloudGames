using Domain.Games;
using Domain.Roles;
using Web.Api.Extensions;

namespace Web.Api.Endpoints.Metadata;

public sealed class Genres : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("metadata/genres", () =>
        {
            var genres = Enum.GetValues<GameGenre>()
            .Select(g => new
            {
                Id = (int)g,
                Name = g.ToString()
            }).ToArray();

            return Results.Ok(genres);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Metadata)
        .WithSummary("Get the list of available game genres.")
        .WithDescription("""
           Returns a list of all predefined game genres available in the system.
           
           Each genre includes:
           - `Id`: The integer value of the enum.
           - `Name`: The string name of the genre.
           
           Useful for populating dropdowns or filters in the UI.
           
           **Available to all authenticated users.**
        """);
    }
}