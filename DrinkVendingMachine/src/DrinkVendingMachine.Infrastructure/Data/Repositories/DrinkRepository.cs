using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkVendingMachine.Infrastructure.Data.Repositories;

public class DrinkRepository(ApplicationDbContext context) : BaseRepository<Drink>(context), IDrinkRepository
{
    public override async Task<IEnumerable<Drink>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await Context.Drinks
            .AsNoTracking()
            .Include(d => d.Brand)
            .ToListAsync(cancellationToken: cancellationToken);

    public override async Task<Drink?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await Context.Drinks
            .Include(d => d.Brand)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken);

    public async Task<IEnumerable<Drink>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default) =>
        await Context.Drinks
            .AsNoTracking()
            .Include(d => d.Brand)
            .Where(d => d.BrandId == brandId)
            .ToListAsync(cancellationToken: cancellationToken);
}