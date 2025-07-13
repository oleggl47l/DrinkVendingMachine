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

    public async Task<List<Drink>> GetFilteredAsync(int? brandId, int? minPrice, int? maxPrice,
        CancellationToken cancellationToken)
    {
        var query = Context.Drinks
            .AsNoTracking()
            .Include(d => d.Brand)
            .AsQueryable();

        if (brandId.HasValue)
            query = query.Where(d => d.BrandId == brandId);

        if (minPrice.HasValue)
            query = query.Where(d => d.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(d => d.Price <= maxPrice.Value);

        return await query.ToListAsync(cancellationToken);
    }
}