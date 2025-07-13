using DrinkVendingMachine.Application.DTOs.Drink;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Exceptions.Brand;
using DrinkVendingMachine.Domain.Exceptions.Drink;
using DrinkVendingMachine.Domain.Interfaces;

namespace DrinkVendingMachine.Application.Services;

public class DrinkService(IDrinkRepository drinkRepository, IBrandRepository brandRepository) : IDrinkService
{
    public async Task<List<DrinkModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var drinks = await drinkRepository.GetAllAsync(cancellationToken);
        return drinks.Select(MapToModel).ToList();
    }

    public async Task<DrinkModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new DrinkNotFoundException(id);
        return MapToModel(drink);
    }

    public async Task<List<DrinkModel>> GetByBrandAsync(int brandId, CancellationToken cancellationToken)
    {
        var drinks = await drinkRepository.GetByBrandAsync(brandId, cancellationToken) ??
                     throw new BrandNotFoundException(brandId);
        return drinks.Select(MapToModel).ToList();
    }

    public async Task UpdateQuantityAsync(int id, int quantity, CancellationToken cancellationToken)
    {
        if (quantity <= 0)
            throw new InvalidDrinkPriceException(quantity);

        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new DrinkNotFoundException(id);

        drink.Quantity = quantity;
        await drinkRepository.UpdateAsync(drink, cancellationToken);
    }

    public async Task AddAsync(DrinkCreateModel model, CancellationToken cancellationToken)
    {
        if (model.Price <= 0)
            throw new InvalidDrinkPriceException(model.Price);

        if (model.Quantity < 0)
            throw new InvalidDrinkQuantityException(model.Quantity);

        var drink = new Drink
        {
            Name = model.Name,
            Price = model.Price,
            Quantity = model.Quantity,
            ImageUrl = model.ImageUrl,
            BrandId = model.BrandId,
        };

        await drinkRepository.AddAsync(drink, cancellationToken);
    }

    public async Task UpdateAsync(DrinkUpdateModel model, CancellationToken cancellationToken)
    {
        var existing = await drinkRepository.GetByIdAsync(model.Id, cancellationToken)
                       ?? throw new DrinkNotFoundException(model.Id);

        if (model.Price is <= 0)
            throw new InvalidDrinkPriceException(model.Price.Value);

        if (model.Quantity is < 0)
            throw new InvalidDrinkQuantityException(model.Quantity.Value);

        if (model.BrandId.HasValue &&
            !await brandRepository.ExistsAsync(model.BrandId.Value, cancellationToken))
            throw new BrandNotFoundException(model.BrandId.Value);

        if (model.Name is not null) existing.Name = model.Name;
        if (model.Price.HasValue) existing.Price = model.Price.Value;
        if (model.Quantity.HasValue) existing.Quantity = model.Quantity.Value;
        if (model.ImageUrl is not null) existing.ImageUrl = model.ImageUrl;
        if (model.BrandId.HasValue) existing.BrandId = model.BrandId.Value;

        await drinkRepository.UpdateAsync(existing, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new DrinkNotFoundException(id);

        await drinkRepository.DeleteAsync(drink, cancellationToken);
    }

    private static DrinkModel MapToModel(Drink drink) =>
        new(
            drink.Id,
            drink.Name,
            drink.Price,
            drink.ImageUrl,
            drink.Quantity,
            drink.Brand.Name
        );
}