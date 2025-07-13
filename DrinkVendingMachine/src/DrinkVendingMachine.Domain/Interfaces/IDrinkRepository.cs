using DrinkVendingMachine.Domain.Entities;

namespace DrinkVendingMachine.Domain.Interfaces;

public interface IDrinkRepository : IRepository<Drink>
{
    Task<IEnumerable<Drink>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default);
    Task<List<Drink>> GetFilteredAsync(int? brandId, int? minPrice, int? maxPrice, CancellationToken cancellationToken);
}