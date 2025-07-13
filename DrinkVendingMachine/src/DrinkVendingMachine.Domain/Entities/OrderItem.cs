namespace DrinkVendingMachine.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public string DrinkName { get; set; } = null!;
    public string BrandName { get; set; } = null!;
    public int PriceAtPurchase { get; set; }
    public int Quantity { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}