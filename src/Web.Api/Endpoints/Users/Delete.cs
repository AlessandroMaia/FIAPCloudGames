using Application.Abstractions.Messaging;
using Application.Users.Commands.Delete;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("users/{userId:guid}", async (
            [FromRoute] Guid userId,
            ICommandHandler<DeleteUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteUserCommand(userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Users)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Delete a user by ID.")
        .WithDescription("""
            Deletes a user identified by the provided `Id`.
            
            - The user ID should be passed as a query parameter.
            - Returns 204 No Content if the user was successfully deleted.
            - Returns an error if the user does not exist or the deletion fails.
            
            **Requires administrator permissions.**
         """);
    }
}
