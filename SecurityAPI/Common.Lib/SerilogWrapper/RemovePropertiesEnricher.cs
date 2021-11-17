using Serilog.Core;
using Serilog.Events;

namespace Common.Lib.SerilogWrapper;

public class RemovePropertiesEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.RemovePropertyIfPresent("RequestId");
        logEvent.RemovePropertyIfPresent("RequestPath");
    }
}