namespace DrinkVendingMachine.Domain.Entities;

public class Drink
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
    
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
}