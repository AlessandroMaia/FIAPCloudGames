using Application.Abstractions.Hubs;
using Application.Abstractions.Messaging;
using Application.Users.Commands.ForceLogout;
using Domain.Roles;
using Microsoft.AspNetCore.SignalR;
using Web.Api.Extensions;
using Web.Api.Hubs;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Auth;

internal sealed class ForceLogout : IEndpoint
{
    public sealed record Request(Guid UserId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/force-logout", async (
            Request request,
            HttpContext context,
            IConfiguration configuration,
            ICommandHandler<ForceLogoutUserCommand> handler,
            IHubContext<ForceLogoutHub, IForceLogoutHub> hubContext,
            CancellationToken cancellationToken) =>
        {

            var command = new ForceLogoutUserCommand(request.UserId);

            var response = await handler.Handle(command, cancellationToken);

            await hubContext
                .Clients
                .User(request.UserId.ToString())
                .ForceLogout(request.UserId);

            return response.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Auth)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Force logout of a specific user.")
        .WithDescription("""
           This endpoint allows an administrator to forcibly log out a specific user from the system.
           
           When invoked, it:
           - Sends a command to process the forced logout action;
           - Uses SignalR to notify the user's active session (if any) about the logout;
           - Returns HTTP 204 (No Content) if successful, or a detailed error if the operation fails.
           
           **Requires administrator permissions.**
        """);
    }
}
