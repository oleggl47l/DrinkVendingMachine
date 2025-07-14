namespace DrinkVendingMachine.Domain.Exceptions.Specific;

public class NotEnoughMoneyInsertedException(int required, int inserted) : CustomException(
    new CustomExceptionArgument(nameof(required), required),
    new CustomExceptionArgument(nameof(inserted), inserted)
);