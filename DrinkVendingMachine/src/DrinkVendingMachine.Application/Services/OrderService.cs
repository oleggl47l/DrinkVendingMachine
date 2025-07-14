using DrinkVendingMachine.Application.DTOs.Order;
using DrinkVendingMachine.Application.Services.Interfaces;
using DrinkVendingMachine.Domain.Entities;
using DrinkVendingMachine.Domain.Exceptions.Brand;
using DrinkVendingMachine.Domain.Exceptions.Coin;
using DrinkVendingMachine.Domain.Exceptions.Drink;
using DrinkVendingMachine.Domain.Exceptions.Specific;
using DrinkVendingMachine.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkVendingMachine.Application.Services;

public class OrderService(
    ICoinRepository coinRepository,
    IDrinkRepository drinkRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<OrderResultDto> CreateOrderAsync(OrderCreateModel model, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var drinks = (await drinkRepository.GetWhereAsync(
                d => model.Items.Select(i => i.DrinkId).Contains(d.Id),
                include: q => q.Include(d => d.Brand),
                cancellationToken
            )).ToList();

            var availableCoins = (await coinRepository.GetAllAsync(cancellationToken)).ToList();

            var drinksDict = drinks.ToDictionary(d => d.Id);
            var coinsDict = availableCoins.ToDictionary(c => c.Id);

            var orderTotal = model.Items.Sum(item =>
            {
                if (!drinksDict.TryGetValue(item.DrinkId, out var drink))
                    throw new DrinkNotFoundException(item.DrinkId);

                if (drink.Quantity < item.Quantity)
                    throw new NotEnoughDrinkStockException(drink.Name);

                return drink.Price * item.Quantity;
            });


            var insertedTotal = model.CoinsInserted.Sum(coinModel =>
            {
                if (!coinsDict.TryGetValue(coinModel.Id, out var coin))
                    throw new CoinNotFoundException(coinModel.Id);

                return coin.Nominal * coinModel.Quantity;
            });

            if (insertedTotal < orderTotal)
                throw new NotEnoughMoneyInsertedException(orderTotal, insertedTotal);

            foreach (var coinModel in model.CoinsInserted)
            {
                var coin = coinsDict[coinModel.Id];
                coin.Quantity += coinModel.Quantity;
                await coinRepository.UpdateAsync(coin, cancellationToken);
            }

            foreach (var item in model.Items)
            {
                var drink = drinksDict[item.DrinkId];
                drink.Quantity -= item.Quantity;
                await drinkRepository.UpdateAsync(drink, cancellationToken);
            }

            var changeAmount = insertedTotal - orderTotal;
            var changeToGive = CalculateChange(changeAmount, availableCoins);

            foreach (var (nominal, qtyToGive) in changeToGive)
            {
                var remainingToGive = qtyToGive;

                var coinSlots = availableCoins
                    .Where(c => c.Nominal == nominal && c.Quantity > 0)
                    .ToList();

                foreach (var coinSlot in coinSlots)
                {
                    if (remainingToGive <= 0) break;

                    var taken = Math.Min(remainingToGive, coinSlot.Quantity);
                    coinSlot.Quantity -= taken;
                    remainingToGive -= taken;

                    await coinRepository.UpdateAsync(coinSlot, cancellationToken);
                }
            }

            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
                Items = model.Items.Select(item =>
                {
                    var drink = drinks.First(d => d.Id == item.DrinkId);
                    return new OrderItem
                    {
                        DrinkName = drink.Name,
                        BrandName = drink.Brand.Name,
                        PriceAtPurchase = drink.Price,
                        Quantity = item.Quantity
                    };
                }).ToList()
            };

            await orderRepository.AddAsync(order, cancellationToken);

            return new OrderResultDto(
                "Спасибо за вашу покупку, пожалуйста, возьмите вашу сдачу.",
                changeToGive,
                changeAmount
            );
        }, cancellationToken);
    }

    private static Dictionary<int, int> CalculateChange(int changeAmount, List<Coin> availableCoins)
    {
        var denomGroups = availableCoins
            .GroupBy(c => c.Nominal)
            .Select(g => (Nominal: g.Key, Quantity: g.Sum(c => c.Quantity)))
            .OrderBy(d => d.Nominal)
            .ToList();

        const int inf = int.MaxValue / 2;

        var dp = new int[changeAmount + 1];
        var prev = new (int denom, int used)[changeAmount + 1];
        for (var s = 1; s <= changeAmount; s++) dp[s] = inf;

        foreach (var (denom, qty) in denomGroups)
        {
            var k = 1;
            var count = qty;
            while (count > 0)
            {
                var take = Math.Min(k, count);
                var coinValue = take * denom;
                for (var s = changeAmount; s >= coinValue; s--)
                {
                    if (dp[s - coinValue] + take < dp[s])
                    {
                        dp[s] = dp[s - coinValue] + take;
                        prev[s] = (denom, take);
                    }
                }

                count -= take;
                k <<= 1;
            }
        }

        if (dp[changeAmount] >= inf)
            throw new UnableToGiveChangeException(changeAmount);

        var result = new Dictionary<int, int>();
        var cur = changeAmount;
        while (cur > 0)
        {
            var (denom, used) = prev[cur];
            if (!result.TryAdd(denom, used))
                result[denom] += used;

            cur -= denom * used;
        }

        return result;
    }
}