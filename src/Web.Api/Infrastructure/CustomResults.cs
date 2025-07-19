using SharedKernel;

namespace Web.Api.Infrastructure;

public static class CustomResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            //title: GetTitle(result.Error),
            statusCode: GetStatusCode(result.Error.Type),
            extensions: GetErrors(result));

        //static string GetTitle(Error error) =>
        //    error.Type switch
        //    {
        //        ErrorType.Validation => error.Code,
        //        ErrorType.Problem => error.Code,
        //        ErrorType.NotFound => error.Code,
        //        ErrorType.Conflict => error.Code,
        //        _ => "Server failure"
        //    };


        static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

        static Dictionary<string, object?>? GetErrors(Result result)
        {
            if (result.Error is ValidationError validationError)
            {
                return new Dictionary<string, object?>
                {
                    { "errors", validationError.Errors }
                };
            }
            else if (result.Error is not null && result.Error is not ValidationError)
            {
                return new Dictionary<string, object?>
                {
                    { "message", result.Error.Message }
                };
            }

            return null;
        }
    }
}
