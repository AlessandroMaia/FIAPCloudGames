using Application.Abstractions.Authentication;
using Domain.Roles;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

internal sealed class UserContext(
    IHttpContextAccessor httpContextAccessor, 
    ITokenProvider tokenProvider) 
        : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid UserId
    {
        get
        {
            if (_httpContextAccessor.HttpContext?.User is null)
                throw new ApplicationException("User context is unavailable");

            var id = tokenProvider.GetUserId(_httpContextAccessor.HttpContext.User);

            if (Guid.TryParse(id, out Guid parsedId))
                return parsedId;

            throw new ApplicationException("User context is unavailable");
        }
    }
        

    public bool IsAdmin =>
        _httpContextAccessor
            .HttpContext?
            .User
            .IsInRole(Role.Admin) ??
        throw new ApplicationException("User context is unavailable");

}
