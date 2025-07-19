using SharedKernel;

namespace Domain.Games;

public static class GameErrors
{
    public static Error NotFound(Guid gameId) =>
        Error.NotFound($"Game with ID '{gameId}' was not found.");

    public static readonly Error TitleNotUnique = Error.Conflict("The title provided is not unique.");

    public static Error AlreadyInLibrary(Guid gameId) =>
        Error.Conflict($"Game with ID '{gameId}' is already in the user's library.");
}
