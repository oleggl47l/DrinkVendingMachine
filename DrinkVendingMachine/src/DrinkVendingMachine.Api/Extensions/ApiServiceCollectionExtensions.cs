using DrinkVendingMachine.Api.Handlers;

namespace DrinkVendingMachine.Api.Extensions;

public static class ApiServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddProblemDetails();
        services.AddEndpointsApiExplorer();
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}