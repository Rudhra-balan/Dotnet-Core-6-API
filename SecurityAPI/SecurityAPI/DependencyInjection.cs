using SecurityAPI.BusinessManager;
using SecurityAPI.BusinessManager.Interfaces;
using SecurityAPI.Repo;
using SecurityAPI.Repo.Interfaces;
using SecurityAPI.Service;
using SecurityAPI.Service.HubService;
using SecurityAPI.Service.HubService.Interface;

namespace SecurityAPI;

public static class DependencyInjection
{
    public static void AddBusinessManager(this IServiceCollection services)
    {
        services.AddScoped<ISecurityBM, SecurityBM>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton<IAsynchronousSignalRBM, AsynchronousSignalRBM>();
        services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
    }

    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<ISecurityRepo, SecurityRepo>();
    }
}