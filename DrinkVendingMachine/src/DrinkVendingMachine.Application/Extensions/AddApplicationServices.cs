using DrinkVendingMachine.Application.Services;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Interfaces;
using DrinkVendingMachine.Infrastructure.Data;
using DrinkVendingMachine.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DrinkVendingMachine.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDrinkRepository, DrinkRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICoinRepository, CoinRepository>();
        services.AddScoped<IDrinkService, DrinkService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}