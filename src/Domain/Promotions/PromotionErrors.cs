using SharedKernel;

namespace Domain.Promotions;

public static class PromotionErrors
{
    public static Error NotFound(Guid promotionId) =>
        Error.NotFound($"Promotion with ID '{promotionId}' was not found.");

    public static readonly Error NameNotUnique = Error.Conflict(
        "The name provided is not unique in active promotion.");
}
