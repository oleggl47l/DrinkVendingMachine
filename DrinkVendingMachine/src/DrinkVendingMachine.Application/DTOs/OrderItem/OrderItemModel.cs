namespace DrinkVendingMachine.Application.DTOs.OrderItem;

public record OrderItemModel(string DrinkName, string BrandName, int PriceAtPurchase, int QuantityAtPurchase);