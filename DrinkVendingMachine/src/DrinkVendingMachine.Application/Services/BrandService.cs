using DrinkVendingMachine.Application.DTOs.Brand;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;

namespace DrinkVendingMachine.Application.Services;

public class BrandService(IBrandRepository brandRepository) : IBrandService
{
    public async Task<List<BrandModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var brands = await brandRepository.GetAllAsync(cancellationToken);
        return brands.Select(MapToModel).ToList();
    }

    public async Task<BrandModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetByIdAsync(id, cancellationToken);
        return brand is null ? null : MapToModel(brand);
    }

    public async Task AddAsync(BrandCreateModel model, CancellationToken cancellationToken)
    {
        var isUnique = await brandRepository.IsNameUniqueAsync(model.Name, cancellationToken);
        if (!isUnique)
            throw new InvalidOperationException("Brand name must be unique.");
        
        var brand = new Brand
        {
            Name = model.Name
        };

        await brandRepository.AddAsync(brand, cancellationToken);
    }

    public async Task UpdateAsync(BrandUpdateModel model, CancellationToken cancellationToken)
    {
        var isUnique = await brandRepository.IsNameUniqueAsync(model.Name, cancellationToken);
        if (!isUnique)
            throw new InvalidOperationException("Brand name must be unique.");
        
        var brand = new Brand
        {
            Id = model.Id,
            Name = model.Name
        };

        await brandRepository.UpdateAsync(brand, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetByIdAsync(id, cancellationToken);
        if (brand is not null)
            await brandRepository.DeleteAsync(brand, cancellationToken);
    }

    public Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken) =>
        brandRepository.IsNameUniqueAsync(name, cancellationToken);

    private static BrandModel MapToModel(Brand brand) => new(brand.Id, brand.Name);
}