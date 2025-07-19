using Application.Abstractions.Messaging;
using Application.Users.Commands.Register;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Auth;

internal sealed class Register : IEndpoint
{
    public sealed record Request(string Email, string Name, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", async (
            Request request,
            ICommandHandler<RegisterUserCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.Name,
                request.Password);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Auth)
        .Produces<Guid>(StatusCodes.Status200OK)
        .WithSummary("Register a new user.")
        .WithDescription("""
           Creates a new user account with the provided name, email, and password.
           
           - Accepts user registration data in the request body.
           - Returns 200 OK with the user ID on successful registration.
           - Returns a problem response if registration fails.
        """);
    }
}
