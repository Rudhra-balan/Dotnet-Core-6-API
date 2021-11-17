using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;


namespace Common.Lib;

/// <summary>
/// Add static service resolver to use when dependencies injection is not available
/// </summary>
public static class ServiceActivator
{
    private static IServiceProvider _serviceProvider;

    /// <summary>
    /// Configure ServiceActivator with full serviceProvider
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void Configure(this IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Create a scope where use this ServiceActivator
    /// </summary>
    /// <returns></returns>
    public static IServiceScope GetScope()
    {
        var provider = _serviceProvider;
        return provider
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
    }

    public static ILogger GetLogger()
    {
        return _serviceProvider.GetRequiredService<ILogger>();
    }

    public static Serilog.ILogger GetSerilogLogger()
    {
        return _serviceProvider.GetRequiredService<Serilog.ILogger>();
    }

    public static IStringLocalizer GetResource()
    {
        return _serviceProvider.GetRequiredService<IStringLocalizer<ResponseMessage>>();
    }


    public static IServiceProvider IServiceProvider => _serviceProvider;
}