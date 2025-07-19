using Microsoft.OpenApi.Models;

namespace Web.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(type =>
            {
                if (!type.IsGenericType)
                {
                    var parts = type.FullName!.Split('.');
                    var lastTwoParts = parts.Skip(parts.Length - 2);
                    return string.Join('.', lastTwoParts).Replace("+", "");
                }

                var genericTypeName = type.Name; 
                var genericTypeNameClean = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));

                var genericArgs = type.GetGenericArguments()
                                      .Select(t => t.Name)
                                      .ToArray();

                return $"{genericTypeNameClean}<{string.Join(",", genericArgs)}>";
            });


            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FIAP Cloud Games (FCG)",
                Description = """
                It is a platform for selling digital games and managing game libraries. 
                
                Authentication is based on JWT tokens stored in HttpOnly cookies for enhanced security.
                """,
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Alessandro",
                    Email = "alessandro.dev@hotmail.com.br"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/license/mit/")
                }
            });
        });

        return services;
    }
}
