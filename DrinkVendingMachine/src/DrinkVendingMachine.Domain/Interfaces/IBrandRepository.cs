using DrinkVendingMachine.Domain.Entities;

namespace DrinkVendingMachine.Domain.Interfaces;

public interface IBrandRepository : IRepository<Brand>
{
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default);
}