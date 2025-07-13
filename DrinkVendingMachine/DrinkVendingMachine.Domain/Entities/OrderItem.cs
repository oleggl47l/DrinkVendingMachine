namespace DrinkVendingMachine.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public required string DrinkName { get; set; }
    public required string BrandName { get; set; }
    public decimal PriceAtPurchase { get; set; }
    public int Quantity { get; set; }

    public int OrderId { get; set; }
    public required Order Order { get; set; }
}