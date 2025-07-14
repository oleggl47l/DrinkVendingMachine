namespace DrinkVendingMachine.Domain.Exceptions.Order;

public class OrderNotFoundException(int id) : CustomException(new CustomExceptionArgument(nameof(id), id));