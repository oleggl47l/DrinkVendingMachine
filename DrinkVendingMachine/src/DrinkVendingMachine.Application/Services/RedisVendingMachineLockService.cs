using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace DrinkVendingMachine.Application.Services;

public class RedisVendingMachineLockService(IDistributedCache cache) : IVendingMachineLockService
{
    private const string LockKey = "vending_machine_lock";
    private readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(30);

    public async Task<bool> TryAcquireLockAsync(string clientId)
    {
        var existing = await cache.GetStringAsync(LockKey);
        if (!string.IsNullOrEmpty(existing) && existing != clientId)
            return false;

        await cache.SetStringAsync(LockKey, clientId, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _lockTimeout
        });

        return true;
    }

    public async Task RefreshLockAsync(string clientId)
    {
        var existing = await cache.GetStringAsync(LockKey);
        if (existing == clientId)
        {
            await cache.SetStringAsync(LockKey, clientId, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _lockTimeout
            });
        }
    }

    public async Task<bool> IsLockedAsync()
    {
        var existing = await cache.GetStringAsync(LockKey);
        return !string.IsNullOrEmpty(existing);
    }

    public async Task ReleaseLockAsync(string clientId)
    {
        var existing = await cache.GetStringAsync(LockKey);
        if (existing == clientId)
            await cache.RemoveAsync(LockKey);
    }
}
