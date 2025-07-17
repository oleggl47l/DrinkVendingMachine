using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkVendingMachine.Infrastructure.Data.Repositories;

public class CoinRepository(ApplicationDbContext context) : BaseRepository<Coin>(context), ICoinRepository
{
    public async Task<IEnumerable<Coin>> GetAllSortedByNominalAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Coin>()
            .Where(c => !c.IsBlocked)
            .AsNoTracking()
            .OrderBy(c => c.Nominal)
            .ToListAsync(cancellationToken);
    }
}