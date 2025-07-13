namespace DrinkVendingMachine.Domain.Entities;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Drink> Drinks { get; set; } = new List<Drink>();
}