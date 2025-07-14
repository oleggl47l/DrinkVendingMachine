using DrinkVendingMachine.Api.Options;
using Microsoft.OpenApi.Models;

namespace DrinkVendingMachine.Api.Extensions;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection services, SwaggerDocOptions options)
    {
        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc(options.Name, new OpenApiInfo
            {
                Version = options.Version,
                Title = options.Title,
                Description = options.Description
            });
            o.EnableAnnotations();
        });
    }
}