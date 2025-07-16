using DrinkVendingMachine.Api.Options;

namespace DrinkVendingMachine.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseApiMiddleware(this IApplicationBuilder app, IWebHostEnvironment env,
        SwaggerDocOptions swaggerOptions)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{swaggerOptions.Name}/swagger.json",
                    $"{swaggerOptions.Title} {swaggerOptions.Version}");
            });
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseCors("AllowFrontend");
    }
}