namespace Web.Api.Extensions;

public static class CookieExtensions
{
    public static void AppendAuthenticationCookies(
        this HttpResponse response,
        IConfiguration configuration,
        string accessToken,
        string refreshToken)
    {
        response.Cookies.Append(configuration.GetValue<string>("AuthKeys:AccessToken")!, accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Domain = null,
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
        });

        response.Cookies.Append(configuration.GetValue<string>("AuthKeys:RefreshToken")!, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Domain = null,
            Expires = DateTime.UtcNow.AddDays(7),
        });
    }

    public static void DeleteAuthenticationCookies(this HttpResponse response, IConfiguration configuration)
    {
        response.Cookies.Delete(configuration.GetValue<string>("AuthKeys:AccessToken")!);
        response.Cookies.Delete(configuration.GetValue<string>("AuthKeys:RefreshToken")!);
    }

    public static (string? token, string? refreshToken) GetAuthenticationCookies(this HttpRequest request, IConfiguration configuration)
    {
        var token = request.Cookies[configuration.GetValue<string?>("AuthKeys:AccessToken") ?? string.Empty];
        var refreshToken = request.Cookies[configuration.GetValue<string>("AuthKeys:RefreshToken") ?? string.Empty];

        return (token, refreshToken);
    }
}
