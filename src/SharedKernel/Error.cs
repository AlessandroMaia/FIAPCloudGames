namespace SharedKernel;

public record Error
{
    public Error(string message, ErrorType type)
    {
        Message = message;
        Type = type;
    }

    public string Message { get; }
    public ErrorType Type { get; }


    public static readonly Error None = new(string.Empty, ErrorType.Failure);

    public static readonly Error NullValue = new("Null value was provided", ErrorType.Failure);

    public static Error Failure(string message) =>
        new(message, ErrorType.Failure);

    public static Error NotFound(string message) =>
        new(message, ErrorType.NotFound);

    public static Error Problem(string message) =>
    new(message, ErrorType.Problem);

    public static Error Conflict(string message) =>
        new(message, ErrorType.Conflict);
}
