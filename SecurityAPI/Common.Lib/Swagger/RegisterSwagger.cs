using Common.Lib.JwtTokenHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.Lib.Swagger;

public static class RegisterSwagger
{
    public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var swaggerSettings = configuration.GetSection(nameof(Swagger)).Get<Swagger>();
        services.Configure<TokenSettings>(configuration.GetSection(nameof(Swagger)));
        services.AddSwaggerGen(s =>
        {
            s.SchemaFilter<SwaggerExcludeFilter>();
            s.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo
            {
                Version = swaggerSettings.Version,
                Title = swaggerSettings.Title,
                Description = swaggerSettings.Description
            });
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer <Key>\"",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));
        var swaggerSettings = configuration.GetSection(nameof(Swagger)).Get<Swagger>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{swaggerSettings.Version}/swagger.json", swaggerSettings.Name);
            c.RoutePrefix = "swagger";
            c.DefaultModelsExpandDepth(-1);
        });
    }
}