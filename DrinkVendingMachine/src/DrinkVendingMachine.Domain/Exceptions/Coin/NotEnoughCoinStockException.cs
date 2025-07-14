namespace DrinkVendingMachine.Domain.Exceptions.Coin;

public class NotEnoughCoinStockException(int nominal)
    : CustomException(new CustomExceptionArgument(nameof(nominal), nominal));