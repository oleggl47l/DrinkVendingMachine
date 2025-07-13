using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkVendingMachine.Infrastructure.Data.Repositories;

public class BrandRepository(ApplicationDbContext context) : BaseRepository<Brand>(context), IBrandRepository
{
    public override async Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await Context.Brands
            .AsNoTracking()
            .Include(b => b.Drinks)
            .ToListAsync(cancellationToken: cancellationToken);

    public override async Task<Brand?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await Context.Brands
            .Include(b => b.Drinks)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken: cancellationToken);

    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default) =>
        !await Context.Brands
            .AsNoTracking()
            .AnyAsync(b => b.Name == name, cancellationToken);
}