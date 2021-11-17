using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Lib.ResponseHandler;

public static class ResponseCompression
{
    public static void AddCustomResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<ResponseGZipSCompressionProvider>();
            options.Providers.Add<ResponseBrotliCompressionProvider>();
        });
    }


    public static void UseCustomResponseCompression(this IApplicationBuilder app)
    {
        app.UseMiddleware<ResponseCompressionQualityMiddleware>(new Dictionary<string, double>
            {
                {"br", 1.0},
                {"gzip", 0.9}
            })
            .UseResponseCompression();
    }
}