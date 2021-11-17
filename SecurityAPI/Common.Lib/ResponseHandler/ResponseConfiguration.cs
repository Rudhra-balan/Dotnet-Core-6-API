using Microsoft.AspNetCore.Builder;

namespace Common.Lib.ResponseHandler;

public static class ResponseConfiguration
{
    public static void UseResponseAndExceptionWrapper(this IApplicationBuilder builder)
    {
        builder.UseMiddleware(typeof(ResponseMiddleware));
    }
}