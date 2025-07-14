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
            var drinks = await drinkRepository.GetWhereAsync(
                d => model.Items.Select(i => i.DrinkId).Contains(d.Id),
                include: q => q.Include(d => d.Brand),
                cancellationToken
            );


            var orderTotal = model.Items.Sum(item =>
            {
                var drink = drinks.FirstOrDefault(d => d.Id == item.DrinkId)
                            ?? throw new DrinkNotFoundException(item.DrinkId);

                if (drink.Quantity < item.Quantity)
                    throw new NotEnoughDrinkStockException(drink.Name);

                return drink.Price * item.Quantity;
            });

            var availableCoins = await coinRepository.GetAllAsync(cancellationToken);

            var insertedTotal = model.CoinsInserted.Sum(coinModel =>
            {
                var coin = availableCoins.FirstOrDefault(c => c.Id == coinModel.Id)
                           ?? throw new CoinNotFoundException(coinModel.Id);

                return coin.Nominal * coinModel.Quantity;
            });

            if (insertedTotal < orderTotal)
                throw new NotEnoughMoneyInsertedException(orderTotal, insertedTotal);

            foreach (var coinModel in model.CoinsInserted)
            {
                var coin = availableCoins.First(c => c.Id == coinModel.Id);
                coin.Quantity += coinModel.Quantity;
                await coinRepository.UpdateAsync(coin, cancellationToken);
            }

            foreach (var item in model.Items)
            {
                var drink = drinks.First(d => d.Id == item.DrinkId);
                drink.Quantity -= item.Quantity;
                await drinkRepository.UpdateAsync(drink, cancellationToken);
            }

            var changeAmount = insertedTotal - orderTotal;

            var changeToGive = new Dictionary<int, int>();
            var sortedNominals = availableCoins
                .Where(c => c.Quantity > 0)
                .Select(c => new { c.Nominal, c.Quantity })
                .OrderByDescending(c => c.Nominal)
                .ToList();

            var remaining = changeAmount;

            foreach (var coin in sortedNominals)
            {
                var needed = remaining / coin.Nominal;
                var toGive = Math.Min(needed, coin.Quantity);

                if (toGive > 0)
                {
                    changeToGive[coin.Nominal] = toGive;
                    remaining -= toGive * coin.Nominal;
                }

                if (remaining == 0)
                    break;
            }

            if (remaining > 0)
                throw new UnableToGiveChangeException(changeAmount);

            foreach (var (nominal, qty) in changeToGive)
            {
                var coin = availableCoins.First(c => c.Nominal == nominal);
                coin.Quantity -= qty;
                await coinRepository.UpdateAsync(coin, cancellationToken);
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
                changeToGive
            );
        }, cancellationToken);
    }
}