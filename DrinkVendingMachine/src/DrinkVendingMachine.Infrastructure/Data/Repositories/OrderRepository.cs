using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkVendingMachine.Infrastructure.Data.Repositories;

public class OrderRepository(ApplicationDbContext context)
    : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetWithItemsAsync(int orderId, CancellationToken cancellationToken = default) =>
        await Context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

    public async Task<List<Order>> GetAllWithItemsAsync(CancellationToken cancellationToken = default) =>
        await Context.Orders
            .Include(o => o.Items)
            .ToListAsync(cancellationToken);
}