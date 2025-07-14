using DrinkVendingMachine.Application.DTOs.Drink;

namespace DrinkVendingMachine.Application.Services.Interfaces;

public interface IDrinkService
{
    Task<List<DrinkModel>> GetAllAsync(DrinkFilterModel filter, CancellationToken cancellationToken);
    Task<DrinkModel?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<DrinkModel>> GetByBrandAsync(int brandId, CancellationToken cancellationToken);
    Task UpdateQuantityAsync(int id, int quantity, CancellationToken cancellationToken);
    Task<DrinkModel> AddAsync(DrinkCreateModel model, CancellationToken cancellationToken);
    Task<DrinkModel> UpdateAsync(DrinkUpdateModel model, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task ImportFromExcelAsync(Stream fileStream, CancellationToken cancellationToken);
}