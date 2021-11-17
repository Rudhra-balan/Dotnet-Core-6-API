using Microsoft.Extensions.DependencyInjection;

namespace Common.Lib.SerilogWrapper;

public static class DependencyInjection
{
    public static void AddLogger(this IServiceCollection services)
    {
        services.AddScoped<Interface.ILogger, Logger>();
    }
}