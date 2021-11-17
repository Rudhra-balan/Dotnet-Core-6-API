namespace Common.Lib.Caching;

public interface ICacheService
{
    T GetOrAdd<T>(string cacheKey, Func<T> factory);
    Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> actionMethod);
}