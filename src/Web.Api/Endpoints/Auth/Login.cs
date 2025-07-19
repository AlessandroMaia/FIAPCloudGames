using Application.Abstractions.Messaging;
using Application.Users.Commands.Login;
using Application.Users.DTOs;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Auth;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async (
            IConfiguration configuration,
            HttpContext context,
            Request request,
            ICommandHandler<LoginUserCommand, CredentialsDto> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            Result<CredentialsDto> response = await handler.Handle(command, cancellationToken);

            if(response is { IsSuccess: true })
            {
                context
                    .Response
                    .AppendAuthenticationCookies(
                        configuration,
                        response.Value.Token,
                        response.Value.RefreshToken
                    );
            }

            return response.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Auth)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Authenticate a user and initiate a session.")
        .WithDescription("""
           Authenticates a user with email and password credentials.
           
           - If successful, sets authentication and refresh tokens as cookies in the response.
           - Returns 204 No Content on success.
           - Returns an error response if credentials are invalid or authentication fails.
        """);
    }
}
