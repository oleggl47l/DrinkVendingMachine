namespace DrinkVendingMachine.Domain.Interfaces;

public interface IUnitOfWork
{
    Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);

    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action,
        CancellationToken cancellationToken = default);
}