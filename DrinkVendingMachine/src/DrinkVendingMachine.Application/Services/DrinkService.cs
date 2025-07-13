using DrinkVendingMachine.Application.DTOs.Drink;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;

namespace DrinkVendingMachine.Application.Services;

public class DrinkService(IDrinkRepository drinkRepository) : IDrinkService
{
    public async Task<List<DrinkModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var drinks = await drinkRepository.GetAllAsync(cancellationToken);
        return drinks.Select(MapToModel).ToList();
    }

    public async Task<DrinkModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken);
        return drink is null ? null : MapToModel(drink);
    }

    public async Task<List<DrinkModel>> GetByBrandAsync(int brandId, CancellationToken cancellationToken)
    {
        var drinks = await drinkRepository.GetByBrandAsync(brandId, cancellationToken);
        return drinks.Select(MapToModel).ToList();
    }

    public async Task UpdateQuantityAsync(int id, int quantity, CancellationToken cancellationToken)
    {
        await drinkRepository.UpdateQuantityAsync(id, quantity, cancellationToken);
    }

    public async Task AddAsync(DrinkCreateModel model, CancellationToken cancellationToken)
    {
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
        var existing = await drinkRepository.GetByIdAsync(model.Id, cancellationToken);
        if (existing is null) return;

        if (model.Name is not null) existing.Name = model.Name;
        if (model.Price.HasValue) existing.Price = model.Price.Value;
        if (model.Quantity.HasValue) existing.Quantity = model.Quantity.Value;
        if (model.ImageUrl is not null) existing.ImageUrl = model.ImageUrl;
        if (model.BrandId.HasValue) existing.BrandId = model.BrandId.Value;

        await drinkRepository.UpdateAsync(existing, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken);
        if (drink is not null)
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