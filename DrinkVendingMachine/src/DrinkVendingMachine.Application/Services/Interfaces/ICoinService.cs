using DrinkVendingMachine.Application.DTOs.Coin;

namespace DrinkVendingMachine.Application.Services.Interfaces;

public interface ICoinService
{
    Task<List<CoinModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<CoinModel?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<CoinModel> AddAsync(CoinCreateModel model, CancellationToken cancellationToken);
    Task<CoinModel> UpdateAsync(CoinUpdateModel model, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}