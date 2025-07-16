namespace DrinkVendingMachine.Application.Services.Interfaces;

public interface IVendingMachineLockService
{
    Task<bool> TryAcquireLockAsync(string clientId);
    Task<bool> IsLockedAsync();
    Task RefreshLockAsync(string clientId);
    Task ReleaseLockAsync(string clientId);
}