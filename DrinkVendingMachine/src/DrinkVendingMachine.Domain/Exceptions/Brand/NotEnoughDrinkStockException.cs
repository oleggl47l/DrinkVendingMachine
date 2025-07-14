namespace DrinkVendingMachine.Domain.Exceptions.Brand;

public class NotEnoughDrinkStockException(string drinkName)
    : CustomException(new CustomExceptionArgument(nameof(drinkName), drinkName));