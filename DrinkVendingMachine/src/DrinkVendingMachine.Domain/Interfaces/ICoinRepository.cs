using DrinkVendingMachine.Domain.Entities;

namespace DrinkVendingMachine.Domain.Interfaces;

public interface ICoinRepository : IRepository<Coin>
{
    Task<IEnumerable<Coin>> GetAllSortedByNominalAsync(CancellationToken cancellationToken = default);
}