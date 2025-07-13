using DrinkVendingMachine.Domain.Entities;

namespace DrinkVendingMachine.Infrastructure.Data.Repositories;

public class OrderItemRepository(ApplicationDbContext context)
    : BaseRepository<OrderItem>(context), IOrderItemRepository;