using DrinkVendingMachine.Application.DTOs.Coin;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Interfaces;

namespace DrinkVendingMachine.Application.Services;

public class CoinService(ICoinRepository coinRepository) : ICoinService
{
    public async Task<List<CoinModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var coins = await coinRepository.GetAllSortedByNominalAsync(cancellationToken);
        return coins.Select(MapToModel).ToList();
    }

    public async Task<CoinModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var coin = await coinRepository.GetByIdAsync(id, cancellationToken);
        if (coin == null)
            throw new KeyNotFoundException($"Coin {id} not found");

        return MapToModel(coin);
    }

    public async Task<CoinModel> AddAsync(CoinCreateModel model, CancellationToken cancellationToken)
    {
        var coin = new Coin
        {
            Nominal = model.Nominal,
            Quantity = model.Quantity,
            IsBlocked = model.IsBlocked
        };

        await coinRepository.AddAsync(coin, cancellationToken);
        return MapToModel(coin);
    }

    public async Task<CoinModel> UpdateAsync(CoinUpdateModel model, CancellationToken cancellationToken)
    {
        var coin = await coinRepository.GetByIdAsync(model.Id, cancellationToken);
        if (coin == null)
            throw new KeyNotFoundException($"Coin {model.Id} not found");

        coin.Nominal = model.Nominal;
        coin.Quantity = model.Quantity;
        coin.IsBlocked = model.IsBlocked;

        await coinRepository.UpdateAsync(coin, cancellationToken);
        return MapToModel(coin);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var coin = await coinRepository.GetByIdAsync(id, cancellationToken);
        if (coin == null)
            throw new KeyNotFoundException($"Coin {id} not found");

        await coinRepository.DeleteAsync(coin, cancellationToken);
    }

    private static CoinModel MapToModel(Coin coin) =>
        new(coin.Id, coin.Nominal, coin.Quantity, coin.IsBlocked);
}