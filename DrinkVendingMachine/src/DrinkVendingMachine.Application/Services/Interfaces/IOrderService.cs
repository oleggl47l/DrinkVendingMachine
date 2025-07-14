using DrinkVendingMachine.Application.DTOs.Order;

namespace DrinkVendingMachine.Application.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderModel>> GetAllOrdersAsync(CancellationToken cancellationToken);
    Task<OrderModel> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken);
    Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken);
    Task<OrderResultDto> CreateOrderAsync(OrderCreateModel model, CancellationToken cancellationToken);
}