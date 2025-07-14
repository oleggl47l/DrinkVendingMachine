using DrinkVendingMachine.Application.DTOs.Order;

namespace DrinkVendingMachine.Application.Services.Interfaces;

public interface IOrderService
{
    Task<OrderResultDto> CreateOrderAsync(OrderCreateModel model, CancellationToken cancellationToken);
}