using DrinkVendingMachine.Api.Extensions;
using DrinkVendingMachine.Api.Handlers;
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
    builder.Services.AddControllers();
    builder.Services.AddProblemDetails();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddSwagger(swaggerDocOptions);

    builder.Services.AddDatabase(configuration);
    builder.Services.AddApplicationServices();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{swaggerDocOptions.Name}/swagger.json",
                $"{swaggerDocOptions.Title} {swaggerDocOptions.Version}");
        });
    }

    app.UseHttpsRedirection();
    app.UseExceptionHandler();
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