namespace DrinkVendingMachine.Domain.Exceptions;

public abstract class CustomException(params CustomExceptionArgument[] args) : Exception
{
    public CustomExceptionArgument[] Args => args;
}