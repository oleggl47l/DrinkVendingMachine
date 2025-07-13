namespace DrinkVendingMachine.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalAmount { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}