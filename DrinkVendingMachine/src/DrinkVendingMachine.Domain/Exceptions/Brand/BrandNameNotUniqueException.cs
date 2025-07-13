namespace DrinkVendingMachine.Domain.Exceptions.Brand;

public class BrandNameNotUniqueException(string name)
    : CustomException(new CustomExceptionArgument(nameof(name), name));