using DrinkVendingMachine.Domain.Entities;

namespace DrinkVendingMachine.Domain.Interfaces;

public interface IDrinkRepository : IRepository<Drink>
{
    Task UpdateQuantityAsync(int id, int quantity, CancellationToken cancellationToken = default);
    Task<IEnumerable<Drink>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default);
}