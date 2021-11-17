using Microsoft.Extensions.Caching.Memory;

namespace Common.Lib.Caching;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> actionMethod)
    {
        // locks get and set internally but call to factory method is not locked
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromHours(2));
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
            return await actionMethod();
        });
    }

    public T GetOrAdd<T>(string cacheKey, Func<T> actionMethod)
    {
        // locks get and set internally but call to factory method is not locked
        return _memoryCache.GetOrCreate(cacheKey, entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromHours(2));
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
            return actionMethod();
        });
    }
}