using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Enrichers;

namespace Common.Lib.SerilogWrapper;

public class SerilogConfiguration
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .Enrich.With(new RemovePropertiesEnricher())
                .Enrich.WithEnvironmentUserNameDelimited()
                .Enrich.WithMachineNameDelimited()
                .Enrich.WithPropertyDelimited("ClassName")
                .Enrich.WithProperty("Version", "1.0.0")
                .ReadFrom.Configuration(context.Configuration);
        };
}

public static class SerilogConfiguationExtention
{
    private const string Delimiter = " | ";

    public static LoggerConfiguration WithEnvironmentUserNameDelimited(
        this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        return enrichmentConfiguration.With(new DelimitedEnricher(new EnvironmentUserNameEnricher(),
            EnvironmentUserNameEnricher.EnvironmentUserNamePropertyName, Delimiter));
    }

    public static LoggerConfiguration WithMachineNameDelimited(
        this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        return enrichmentConfiguration.With(new DelimitedEnricher(new MachineNameEnricher(),
            MachineNameEnricher.MachineNamePropertyName, Delimiter));
    }

    public static LoggerConfiguration WithPropertyDelimited(
        this LoggerEnrichmentConfiguration enrichmentConfiguration, string propertyName)
    {
        return enrichmentConfiguration.With(new DelimitedEnricher(propertyName, Delimiter));
    }
}