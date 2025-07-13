using DrinkVendingMachine.Domain.Entities;

namespace DrinkVendingMachine.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetWithItemsAsync(int orderId, CancellationToken cancellationToken = default);
    Task<List<Order>> GetAllWithItemsAsync(CancellationToken cancellationToken = default);
}