using DrinkVendingMachine.Api.Extensions;
using DrinkVendingMachine.Api.Options;
using DrinkVendingMachine.Application.Extensions;
using DrinkVendingMachine.Infrastructure.Extensions;
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
    builder.Services.AddApiServices();
    builder.Services.AddDatabase(configuration);
    builder.Services.AddApplicationServices();

    var app = builder.Build();
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