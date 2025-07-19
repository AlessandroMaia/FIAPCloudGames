using Application.Abstractions.Messaging;
using Application.Users.Commands.Logout;
using Domain.Roles;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Auth;

internal sealed class Logout : IEndpoint
{

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/logout", async (
            HttpContext context,
            IConfiguration configuration,
            ICommandHandler<LogoutUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var (token, refreshToken) = context.Request.GetAuthenticationCookies(configuration);
            
            var command = new LogoutUserCommand(token, refreshToken);

            var response = await handler.Handle(command, cancellationToken);

            context.Response.DeleteAuthenticationCookies(configuration);

            return response.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Auth)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Logs out the currently authenticated user.")
        .WithDescription("""
           Logs out the currently authenticated user by invalidating the access and refresh tokens.
           
           - Extracts tokens from the authentication cookies.
           - Deletes the authentication cookies from the response.
           - Returns 204 No Content on successful logout.
           
           Available to all authenticated roles.
        """);
    }
}
