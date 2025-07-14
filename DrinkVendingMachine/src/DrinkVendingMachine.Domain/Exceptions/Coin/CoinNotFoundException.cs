namespace DrinkVendingMachine.Domain.Exceptions.Coin;

public class CoinNotFoundException(int id)
    : CustomException(new CustomExceptionArgument(nameof(id), id));