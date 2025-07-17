namespace DrinkVendingMachine.Application.DTOs.Coin;

public record CoinCreateModel(int Nominal, int Quantity, bool IsBlocked);