using Serilog.Core;
using Serilog.Events;

namespace Common.Lib.SerilogWrapper;

public class DelimitedEnricher : ILogEventEnricher
{
    private readonly string _delimiter;
    private readonly ILogEventEnricher _innerEnricher;
    private readonly string _innerPropertyName;

    public DelimitedEnricher(string innerPropertyName, string delimiter)
    {
        _innerPropertyName = innerPropertyName;
        _delimiter = delimiter;
    }

    public DelimitedEnricher(ILogEventEnricher innerEnricher, string innerPropertyName, string delimiter) : this(
        innerPropertyName, delimiter)
    {
        _innerEnricher = innerEnricher;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (_innerEnricher != null)
            _innerEnricher.Enrich(logEvent, propertyFactory);

        if (!logEvent.Properties.TryGetValue(_innerPropertyName, out var eventPropertyValue)) return;
        var value = (eventPropertyValue as ScalarValue)?.Value as string;
        if (!string.IsNullOrEmpty(value))
            logEvent.AddPropertyIfAbsent(new LogEventProperty(_innerPropertyName + "Delimited",
                new ScalarValue(value + _delimiter)));
    }
}