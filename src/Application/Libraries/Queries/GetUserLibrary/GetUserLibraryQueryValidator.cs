using FluentValidation;

namespace Application.Libraries.Queries.GetUserLibrary;

public class GetUserLibraryQueryValidator : AbstractValidator<GetUserLibraryQuery>
{
    public GetUserLibraryQueryValidator()
    {
        RuleFor(command => command.UserId)
        .NotEmpty().WithMessage("User ID is required.");
    }
}
