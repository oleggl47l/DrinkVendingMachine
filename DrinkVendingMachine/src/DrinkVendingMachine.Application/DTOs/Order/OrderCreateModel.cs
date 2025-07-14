using DrinkVendingMachine.Application.DTOs.Coin;
using DrinkVendingMachine.Application.DTOs.OrderItem;

namespace DrinkVendingMachine.Application.DTOs.Order;

public record OrderCreateModel(List<OrderItemPurchaseModel> Items, List<CoinInputModel> CoinsInserted);