using DrinkVendingMachine.Application.DTOs.Coin;
using DrinkVendingMachine.Application.DTOs.OrderItem;

namespace DrinkVendingMachine.Application.DTOs.Order;

public record OrderCreateModel(List<OrderItemModel> Items, List<CoinInputModel> CoinsInserted);