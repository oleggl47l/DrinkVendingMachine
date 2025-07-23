using DrinkVendingMachine.Application.DTOs.Drink;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;
using OfficeOpenXml;

namespace DrinkVendingMachine.Application.Services;

public class DrinkService(IDrinkRepository drinkRepository, IBrandRepository brandRepository) : IDrinkService
{
    public async Task<List<DrinkModel>> GetAllAsync(DrinkFilterModel filter, CancellationToken cancellationToken)
    {
        var drinks =
            await drinkRepository.GetFilteredAsync(filter.BrandId, filter.MinPrice, filter.MaxPrice, cancellationToken);
        return drinks.Select(MapToModel).ToList();
    }

    public async Task<DrinkModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new KeyNotFoundException($"Drink with id={id} not found");
        return MapToModel(drink);
    }

    public async Task<List<DrinkModel>> GetByBrandAsync(int brandId, CancellationToken cancellationToken)
    {
        var drinks = await drinkRepository.GetByBrandAsync(brandId, cancellationToken) 
                     ?? throw new KeyNotFoundException($"Brand with id={brandId} not found");
        return drinks.Select(MapToModel).ToList();
    }

    public async Task UpdateQuantityAsync(int id, int quantity, CancellationToken cancellationToken)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new KeyNotFoundException($"Drink with id={id} not found");

        drink.Quantity = quantity;
        await drinkRepository.UpdateAsync(drink, cancellationToken);
    }

    public async Task<DrinkModel> AddAsync(DrinkCreateModel model, CancellationToken cancellationToken)
    {
        if (model.Price <= 0)
            throw new ArgumentOutOfRangeException(nameof(model), "Price must be greater than 0");

        if (model.Quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(model), "Quantity cannot be negative");

        var drink = new Drink
        {
            Name = model.Name,
            Price = model.Price,
            Quantity = model.Quantity,
            ImageUrl = model.ImageUrl,
            BrandId = model.BrandId,
        };

        await drinkRepository.AddAsync(drink, cancellationToken);
        return (await GetByIdAsync(drink.Id, cancellationToken))!;
    }

    public async Task<DrinkModel> UpdateAsync(DrinkUpdateModel model, CancellationToken cancellationToken)
    {
        var existing = await drinkRepository.GetByIdAsync(model.Id, cancellationToken)
                       ?? throw new KeyNotFoundException($"Drink with id={model.Id} not found");

        if (model.Price is <= 0)
            throw new ArgumentOutOfRangeException(nameof(model), "Price must be greater than 0");

        if (model.Quantity is < 0)
            throw new ArgumentOutOfRangeException(nameof(model), "Quantity cannot be negative");

        if (model.BrandId.HasValue &&
            !await brandRepository.ExistsAsync(model.BrandId.Value, cancellationToken))
            throw new KeyNotFoundException($"Brand with id={model.BrandId.Value} not found");

        if (model.Name is not null) existing.Name = model.Name;
        if (model.Price.HasValue) existing.Price = model.Price.Value;
        if (model.Quantity.HasValue) existing.Quantity = model.Quantity.Value;
        if (model.ImageUrl is not null) existing.ImageUrl = model.ImageUrl;
        if (model.BrandId.HasValue) existing.BrandId = model.BrandId.Value;

        await drinkRepository.UpdateAsync(existing, cancellationToken);
        return (await GetByIdAsync(model.Id, cancellationToken))!;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new KeyNotFoundException($"Drink with id={id} not found");

        await drinkRepository.DeleteAsync(drink, cancellationToken);
    }

    public async Task<PriceRangeModel> GetPriceRangeAsync(int? brandId, CancellationToken cancellationToken)
    {
        var (minPrice, maxPrice) = await drinkRepository.GetPriceRangeAsync(brandId, cancellationToken);
        return new PriceRangeModel(minPrice, maxPrice);
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

    public async Task ImportFromExcelAsync(Stream fileStream, CancellationToken cancellationToken)
    {
        ExcelPackage.License.SetNonCommercialPersonal("<Oleg>");

        using var package = new ExcelPackage(fileStream);

        if (package.Workbook.Worksheets.Count == 0)
            throw new InvalidDataException("Excel file contains no worksheets.");

        var worksheet = package.Workbook.Worksheets[0];

        var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var requiredColumns = new[] { "Name", "BrandId", "Price", "Quantity", "ImageUrl" };
        var columnCount = worksheet.Dimension?.Columns ?? 0;

        for (var col = 1; col <= columnCount; col++)
        {
            var headerText = worksheet.Cells[1, col].Text?.Trim();
            if (!string.IsNullOrEmpty(headerText))
            {
                columnMap[headerText] = col;
            }
        }

        var missingColumns = requiredColumns
            .Where(col => !columnMap.ContainsKey(col))
            .ToArray();

        if (missingColumns.Length > 0)
            throw new InvalidDataException($"Missing required columns: {string.Join(", ", missingColumns)}");

        var duplicateColumns = columnMap
            .GroupBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToArray();

        if (duplicateColumns.Length > 0)
            throw new InvalidDataException($"Duplicate columns in Excel: {string.Join(", ", duplicateColumns)}");

        var rowCount = worksheet.Dimension?.Rows ?? 1;
        var importedDrinks = new List<Drink>();

        for (var row = 2; row <= rowCount; row++)
        {
            try
            {
                var name = worksheet.Cells[row, columnMap["Name"]].Text?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                    throw new FormatException($"Row {row}: 'Name' is empty.");

                var brandIdText = worksheet.Cells[row, columnMap["BrandId"]].Text;
                if (!int.TryParse(brandIdText, out var brandId))
                    throw new FormatException($"Row {row}: Invalid BrandId '{brandIdText}'.");

                var priceText = worksheet.Cells[row, columnMap["Price"]].Text;
                if (!int.TryParse(priceText, out var price) || price <= 0)
                    throw new FormatException($"Row {row}: Invalid Price '{priceText}'.");

                var quantityText = worksheet.Cells[row, columnMap["Quantity"]].Text;
                if (!int.TryParse(quantityText, out var quantity) || quantity < 0)
                    throw new FormatException($"Row {row}: Invalid Quantity '{quantityText}'.");

                var imageUrl = worksheet.Cells[row, columnMap["ImageUrl"]].Text?.Trim();

                if (!await brandRepository.ExistsAsync(brandId, cancellationToken))
                    throw new KeyNotFoundException($"Row {row}: Brand with id={brandId} not found.");

                importedDrinks.Add(new Drink
                {
                    Name = name,
                    BrandId = brandId,
                    Price = price,
                    Quantity = quantity,
                    ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl
                });
            }
            catch (Exception ex)
            {
                throw new FormatException($"Row {row}: Error while processing row — {ex.Message}", ex);
            }
        }

        foreach (var drink in importedDrinks)
            await drinkRepository.AddAsync(drink, cancellationToken);
    }
}