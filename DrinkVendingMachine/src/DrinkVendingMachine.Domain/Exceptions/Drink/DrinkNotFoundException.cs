namespace DrinkVendingMachine.Domain.Exceptions.Drink;

public class DrinkNotFoundException(int id) : CustomException(new CustomExceptionArgument(nameof(id), id));