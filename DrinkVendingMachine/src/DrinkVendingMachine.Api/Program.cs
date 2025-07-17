using DrinkVendingMachine.Api.Extensions;
using DrinkVendingMachine.Api.Options;
using DrinkVendingMachine.Application.Extensions;
using DrinkVendingMachine.Infrastructure.Data;
using DrinkVendingMachine.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build()).WriteTo.Console()
    .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    
    var swaggerDocOptions = configuration.GetSection("SwaggerDocOptions").Get<SwaggerDocOptions>() ??
                            throw new ArgumentNullException(nameof(SwaggerDocOptions));

    builder.Host.UseSerilog();
    builder.Services.AddSwagger(swaggerDocOptions);
    builder.Services.AddApiServices(configuration);
    builder.Services.AddDatabase(configuration);
    builder.Services.AddApplicationServices();

    var app = builder.Build();
    
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
                
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }
    
    app.UseApiMiddleware(app.Environment, swaggerDocOptions);
    app.MapControllers();

    Log.Information("Application started successfully");
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}