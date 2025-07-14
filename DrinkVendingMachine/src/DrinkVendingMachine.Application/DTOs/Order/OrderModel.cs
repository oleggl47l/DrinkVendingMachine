using DrinkVendingMachine.Application.DTOs.OrderItem;

namespace DrinkVendingMachine.Application.DTOs.Order;

public record OrderModel(int Id, DateTime CreatedAt, int TotalAmount, List<OrderItemModel> Items);