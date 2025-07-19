using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Application.Users.Queries.GetById;
using Domain.Roles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{userId:guid}", async (
            Guid userId,
            IQueryHandler<GetUserByIdQuery, UserDetailDto> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserByIdQuery(userId);

            Result<UserDetailDto> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission([Role.Admin])
        .WithTags(Tags.Users)
        .Produces<UserDetailDto>(StatusCodes.Status200OK)
        .WithSummary("Get user details by ID.")
        .WithDescription("""
           Retrieves detailed information about a specific user identified by `userId`.
           
           - The `userId` is provided as a route parameter.
           - Returns 200 OK with user details if found.
           - Returns an error if the user does not exist.
           
           **Requires administrator permissions.**
        """);
    }
}
