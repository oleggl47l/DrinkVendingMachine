using DrinkVendingMachine.Application.DTOs.Brand;

namespace DrinkVendingMachine.Application.Services.Interfaces;

public interface IBrandService
{
    Task<List<BrandModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<BrandModel?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken);
    Task<BrandModel> AddAsync(BrandCreateModel model, CancellationToken cancellationToken);
    Task<BrandModel> UpdateAsync(BrandUpdateModel model, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}