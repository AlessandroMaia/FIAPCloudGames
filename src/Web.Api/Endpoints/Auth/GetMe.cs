using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Application.Users.Queries.GetMe;
using Domain.Roles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Auth;

internal sealed class GetMe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("auth/me", async (
            IQueryHandler<GetMeQuery, UserDetailDto> handler,
            CancellationToken cancellationToken) =>
        {
            Result<UserDetailDto> result = await handler.Handle(new GetMeQuery(), cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Auth)
        .Produces<UserDetailDto>(StatusCodes.Status200OK)
        .WithSummary("Get information about the currently authenticated user.")
        .WithDescription("""
           Retrieves detailed information about the currently authenticated user based on the access token.
           
           - Requires a valid authentication token.
           - Returns 200 OK with user details on success.
           - Returns an error response if the user is not authenticated or something goes wrong.
           
           Available to all authenticated roles.
        """);
    }
}
