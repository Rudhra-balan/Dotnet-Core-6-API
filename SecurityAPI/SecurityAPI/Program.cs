using System.Diagnostics;
using Common.Lib;
using Common.Lib.Caching;
using Common.Lib.Exceptions.ErrorHandler;
using Common.Lib.JwtTokenHandler;
using Common.Lib.ResponseHandler;
using Common.Lib.Security;
using Common.Lib.Security.Headers;
using Common.Lib.SerilogWrapper;
using Common.Lib.Swagger;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using SecurityAPI;
using SecurityAPI.Service.HubService;
using Serilog;
using Serilog.Debugging;

var builder = WebApplication.CreateBuilder(args);

#region [Configure Services]

builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile(
        $"securityapi.appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.log.json",
        true, true);
    config.AddJsonFile("securityapi.appsettings.json", true, true);
    config.AddJsonFile($"securityapi.appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
    config.AddEnvironmentVariables();
});

builder.Host.UseSerilog(SerilogConfiguration.Configure);

if (builder.Environment.IsDevelopment())
    SelfLog.Enable(msg =>
    {
        Debug.Print(msg);
        Debugger.Break();
    });

#region Setup CORS

var corsBuilder = new CorsPolicyBuilder();

corsBuilder.SetIsOriginAllowed(_ => true);
corsBuilder.AllowAnyHeader();
corsBuilder.AllowCredentials();
corsBuilder.AllowAnyMethod();

builder.Services.AddCors(options => {
    options.AddPolicy("_CorsPolicy", corsBuilder.Build());
});

#endregion

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
    options.Filters.Add(new ValidateModelAttribute());
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = _ => new ValidationResult();
});
builder.Services.AddSwaggerConfiguration(builder.Configuration);
builder.Services.AddSignalR(options => {
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromMilliseconds(60);
});
builder.Services.AddCacheService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCustomResponseCompression();
builder.Services.AddJwtTokenAuthentication(builder.Configuration);
builder.Services.AddLogger();
builder.Services.AddBusinessManager();
builder.Services.AddRepository();
builder.Services.AddLocalizationResource();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region [Configure]

//This method gets called by the runtime. Use this method to configure the HTTP request pipeline

var app = builder.Build();

app.UseRouting();
app.Services.Configure();
app.UseSwaggerSetup(builder.Configuration);
app.UseHttpsRedirection();
app.UseResponseAndExceptionWrapper();

app.UseCustomResponseCompression();

app.UseAntiXssMiddleware();
app.UseSecurityHeadersMiddleware(
    new SecurityHeadersBuilder()
        .AddDefaultSecurePolicy());
app.UseDosAttackMiddleware();
app.UseCors("_CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalRContext>("/signalR/notification");
});
app.UseLocalizationResource();

app.Run();

#endregion