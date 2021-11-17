using Microsoft.Extensions.DependencyInjection;

namespace Common.Lib.Caching;

public static class RegisterCacheService
{
    public static void AddCacheService(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
    }
}