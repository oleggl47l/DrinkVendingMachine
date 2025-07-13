namespace DrinkVendingMachine.Domain.Entities;

public class Coin
{
    public int Id { get; set; }
    public int Nominal { get; set; } 
    public int Quantity { get; set; }
    public bool IsBlocked { get; set; }
}