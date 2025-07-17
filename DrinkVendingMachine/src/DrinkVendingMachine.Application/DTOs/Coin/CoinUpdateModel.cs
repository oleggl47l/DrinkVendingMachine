namespace DrinkVendingMachine.Application.DTOs.Coin;

public record CoinUpdateModel(int Id, int Nominal, int Quantity, bool IsBlocked);