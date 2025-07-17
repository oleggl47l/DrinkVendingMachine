namespace DrinkVendingMachine.Application.DTOs.Coin;

public record CoinModel(int Id, int Nominal, int Quantity, bool IsBlocked);