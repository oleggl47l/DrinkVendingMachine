namespace DrinkVendingMachine.Domain.Entities;

public class Brand
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<Drink> Drinks { get; set; } = new List<Drink>();
}