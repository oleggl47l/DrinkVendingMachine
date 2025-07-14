using DrinkVendingMachine.Application.DTOs.Brand;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Exceptions.Brand;
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
        var brand = await brandRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new BrandNotFoundException(id);

        return MapToModel(brand);
    }

    public async Task<BrandModel> AddAsync(BrandCreateModel model, CancellationToken cancellationToken)
    {
        var isUnique = await brandRepository.IsNameUniqueAsync(model.Name, cancellationToken);
        if (!isUnique)
            throw new BrandNameNotUniqueException(model.Name);

        var brand = new Brand { Name = model.Name };
        await brandRepository.AddAsync(brand, cancellationToken);

        return MapToModel(brand);
    }

    public async Task<BrandModel> UpdateAsync(BrandUpdateModel model, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetByIdAsync(model.Id, cancellationToken)
                    ?? throw new BrandNotFoundException(model.Id);

        var isUnique = await brandRepository.IsNameUniqueAsync(model.Name, cancellationToken);
        if (!isUnique)
            throw new BrandNameNotUniqueException(model.Name);

        brand.Name = model.Name;
        await brandRepository.UpdateAsync(brand, cancellationToken);

        return MapToModel(brand);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new BrandNotFoundException(id);

        await brandRepository.DeleteAsync(brand, cancellationToken);
    }

    public Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken) =>
        brandRepository.IsNameUniqueAsync(name, cancellationToken);

    private static BrandModel MapToModel(Brand brand) => new(brand.Id, brand.Name);
}