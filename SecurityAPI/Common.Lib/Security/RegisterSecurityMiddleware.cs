using Common.Lib.Security.Headers;
using Microsoft.AspNetCore.Builder;

namespace Common.Lib.Security;

public static class RegisterSecurityMiddleware
{
    public static IApplicationBuilder UseAntiXssMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AntiXssMiddleware>();
    }

    public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app,
        SecurityHeadersBuilder builder)
    {
        var policy = builder.Build();
        return app.UseMiddleware<SecurityHeadersMiddleware>(policy);
    }

    public static IApplicationBuilder UseDosAttackMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DosAttackMiddleware>();
    }
}