using Domain.PurchaseHistories;
using Domain.Roles;
using Web.Api.Extensions;

namespace Web.Api.Endpoints.Metadata;

public sealed class PaymentMethods : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("metadata/payment-methods", () =>
        {
            var methods = Enum.GetValues<PaymentMethod>()
            .Select(g => new
            {
                Id = (int)g,
                Name = g.ToString()
            }).ToArray();

            return Results.Ok(methods);
        })
        .HasPermission(Role.All)
        .WithTags(Tags.Metadata)
        .WithSummary("Get the list of available payment methods.")
        .WithDescription("""
           Returns a list of all supported payment methods in the system.

           Each payment-methods includes:
           - `Id`: The integer value of the enum.
           - `Name`: The string name of the payment-methods.
           
           This is typically used to populate payment options in the frontend.
           
           **Available to all authenticated users.**
        """);
    }
}