namespace DrinkVendingMachine.Domain.Exceptions.Drink;

public class InvalidDrinkPriceException(int price) : CustomException(new CustomExceptionArgument(nameof(price), price));