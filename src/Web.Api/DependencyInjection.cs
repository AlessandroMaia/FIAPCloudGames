using Web.Api.Extensions;
using Web.Api.Hubs;
using Web.Api.Infrastructure;

namespace Web.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGenWithAuth();
        services.AddMemoryCache();
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

       var corsWhitelistedDomains = configuration.GetSection("CORsWhitelistedDomains").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins(corsWhitelistedDomains)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        services.AddSignalR();

        return services;
    }

    public static WebApplication AddHubs(this WebApplication app)
    {
        app.MapHub<ForceLogoutHub>("/hubs/force-logout")
            .RequireAuthorization();

        return app;
    }
}
