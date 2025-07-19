using Application.Abstractions.Messaging;
using Application.Users.Commands.UpdateUserRole;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

public sealed record UpdateUserRoleRequest(string NewRole);

internal sealed class UpdateUserRole : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("users/{userId:guid}/role", async (
            [FromRoute] Guid userId,
            [FromBody]UpdateUserRoleRequest request,
            IConfiguration configuration,
            HttpContext context,
            ICommandHandler<UpdateUserRoleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            UpdateUserRoleCommand command = new(userId, request.NewRole);

            Result response = await handler.Handle(command, cancellationToken);

            return response.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Users)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Update the role of a user.")
        .WithDescription("""
           Updates the role of the user identified by `userId`.
           
           - The new role must be provided in the request body (`NewRole`).
           - Returns 204 No Content if the update is successful.
           - Returns an error response if the update fails.
           
           **Requires administrator permissions.**
        """);
    }
}
