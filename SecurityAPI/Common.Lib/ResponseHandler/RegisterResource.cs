using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Common.Lib.ResponseHandler;

public static class RegisterResource
{
    public static IServiceCollection AddLocalizationResource(this IServiceCollection services)
    {
        services.AddLocalization(config => { config.ResourcesPath = ""; });

        services.Configure<RequestLocalizationOptions>(
            opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new("en"),
                    new("de"),
                    new("es"),
                    new("fr")
                };

                opts.DefaultRequestCulture = new RequestCulture("en");
                // Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;

                opts.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var languages = context.Request.Headers["Accept-Language"].ToString();
                    var currentLanguage = languages.Split(',').FirstOrDefault();
                    var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "en" : currentLanguage.ToLower();

                    if (defaultLanguage != "en" && defaultLanguage != "de" && defaultLanguage != "es" &&
                        defaultLanguage != "fr") defaultLanguage = "en";

                    return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                }));
            });

        return services;
    }

    public static void UseLocalizationResource(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        if (options != null) app.UseRequestLocalization(options.Value);
    }
}