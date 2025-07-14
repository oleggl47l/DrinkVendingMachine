namespace DrinkVendingMachine.Domain.Exceptions.Specific;

public class UnableToGiveChangeException(int changeAmount)
    : CustomException(new CustomExceptionArgument(nameof(changeAmount), changeAmount));