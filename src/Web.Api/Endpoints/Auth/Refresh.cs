using Application.Abstractions.Messaging;
using Application.Users.Commands.Refresh;
using Application.Users.DTOs;
using Domain.Roles;
using Domain.Users;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Auth;

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh", async (
            IConfiguration configuration,
            HttpContext context,
            ICommandHandler<RefreshUserCommand, CredentialsDto> handler,
            CancellationToken cancellationToken) =>
        {
            var refreshToken = context.Request.Cookies[configuration.GetValue<string>("AuthKeys:RefreshToken")!];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return CustomResults.Problem(Result.Failure(UserErrors.RefreshTokenInvalid));
            }

            var command = new RefreshUserCommand(refreshToken);

            Result<CredentialsDto> response = await handler.Handle(command, cancellationToken);

            if (response is { IsSuccess: true })
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
        .HasPermission(Role.All)
        .WithTags(Tags.Auth)
        .Produces(StatusCodes.Status204NoContent)
        .WithSummary("Refresh authentication tokens using a valid refresh token.")
        .WithDescription("""
           Refreshes the user's authentication session by issuing new access and refresh tokens.
           
           - Expects a valid refresh token to be present in the request cookies.
           - If valid, issues new tokens and updates the authentication cookies.
           - Returns 204 No Content on success.
           - Returns an error response if the refresh token is missing or invalid.
           
           Available to all authenticated roles.
        """);
    }
}
