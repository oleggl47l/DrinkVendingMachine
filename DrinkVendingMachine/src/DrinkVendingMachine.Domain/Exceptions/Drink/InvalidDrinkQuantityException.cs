namespace DrinkVendingMachine.Domain.Exceptions.Drink;

public class InvalidDrinkQuantityException(int quantity)
    : CustomException(new CustomExceptionArgument(nameof(quantity), quantity));