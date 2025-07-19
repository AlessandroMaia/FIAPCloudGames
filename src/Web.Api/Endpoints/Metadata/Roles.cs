using Domain.Roles;
using Web.Api.Extensions;

namespace Web.Api.Endpoints.Metadata;

public sealed class Roles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("metadata/roles", () =>
        {
            var roles = Role.AllComplete;

            return Results.Ok(roles);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Metadata)
        .WithSummary("Get the list of all available user roles.")
        .WithDescription("""
           Returns a list of all user roles defined in the system, including their identifiers and display names.
           
           Useful for administrative interfaces where role assignment is needed.
           
           **Requires administrator permissions.**
        """);
    }
}