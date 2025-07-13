namespace DrinkVendingMachine.Domain.Exceptions.Brand;

public class BrandNotFoundException(int id) : CustomException(new CustomExceptionArgument(nameof(id), id));