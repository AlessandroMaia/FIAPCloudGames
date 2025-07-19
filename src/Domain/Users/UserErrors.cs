using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        $"The user with the Id = '{userId}' was not found");

    public static Error Unauthorized() => Error.Failure(
        "You are not authorized to perform this action");

    public static readonly Error Failure = Error.Conflict(
        "Invald email and/or senha password");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "The user with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "The provided email is not unique");

    public static readonly Error RefreshTokenInvalid = Error.Failure(
        "The refresh token has expired or invalidated");
}
