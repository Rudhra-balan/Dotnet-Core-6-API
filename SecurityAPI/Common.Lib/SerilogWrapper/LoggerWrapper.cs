using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace Common.Lib.SerilogWrapper;

public static class LoggerWrapper
{
    private static readonly ILogger logger = ServiceActivator.GetSerilogLogger();

    public static void Error(string applicationMessage)
    {
        if (!Log.IsEnabled(LogEventLevel.Error)) return;
        logger.Error(applicationMessage);
    }

    public static void Error(Exception ex, string applicationMessage)
    {
        if (!Log.IsEnabled(LogEventLevel.Error)) return;
        var exceptionMessage = new ExceptionMessageTemplate(ex)
        {
            ApplicationMessage = applicationMessage
        };
        var json = JsonConvert.SerializeObject(exceptionMessage, Formatting.Indented);
        logger.Error($"{json}", ex);
    }

    public static void Error(Exception ex, int sourceLineNumber = 0,
        string memberName = "",
        string sourceFilePath = ""
    )
    {
        if (!Log.IsEnabled(LogEventLevel.Error)) return;
        var exceptionMessage = new ExceptionMessageTemplate(ex)
        {
            CallerInformation = new LogMessageTemplate
            {
                MemberName = memberName,
                Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
                SourceLineNumber = sourceLineNumber,
                SourceFilePath = sourceFilePath
            }
        };
        var json = JsonConvert.SerializeObject(exceptionMessage, Formatting.Indented);
        logger.Error($"{json}", ex);
    }


    public static void Warn(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!Log.IsEnabled(LogEventLevel.Warning)) return;
        if (ex == null) return;
        var exceptionMessage = new ExceptionMessageTemplate(ex)
        {
            CallerInformation = new LogMessageTemplate
            {
                MemberName = memberName,
                Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
                SourceLineNumber = sourceLineNumber,
                SourceFilePath = sourceFilePath
            }
        };
        var json = JsonConvert.SerializeObject(exceptionMessage, Formatting.Indented);
        Log.Warning($"{json}", ex);
    }

    public static void Warn(string message = "Warning",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!Log.IsEnabled(LogEventLevel.Warning)) return;
        var logMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message
        };
        var json = JsonConvert.SerializeObject(logMessage, Formatting.Indented);
        logger.Warning($"{json}");
    }

    public static void Debug(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!Log.IsEnabled(LogEventLevel.Debug)) return;
        if (ex == null) return;
        var exceptionMessage = new ExceptionMessageTemplate(ex)
        {
            CallerInformation = new LogMessageTemplate
            {
                MemberName = memberName,
                Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
                SourceLineNumber = sourceLineNumber,
                SourceFilePath = sourceFilePath
            }
        };
        var json = JsonConvert.SerializeObject(exceptionMessage, Formatting.Indented);
        logger.Debug($"{json}", ex);
    }

    public static void Debug(string message = "Debug",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!Log.IsEnabled(LogEventLevel.Debug)) return;
        var logMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message
        };
        var json = JsonConvert.SerializeObject(logMessage, Formatting.Indented);
        logger.Debug($"{json}");
    }

    public static void Info(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!Log.IsEnabled(LogEventLevel.Information)) return;
        if (ex == null) return;
        var exceptionMessage = new ExceptionMessageTemplate(ex)
        {
            CallerInformation = new LogMessageTemplate
            {
                MemberName = memberName,
                Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
                SourceLineNumber = sourceLineNumber,
                SourceFilePath = sourceFilePath
            }
        };
        var json = JsonConvert.SerializeObject(exceptionMessage, Formatting.Indented);
        logger.Information($"{json}", ex);
    }

    public static void Info(string message = "Information",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!Log.IsEnabled(LogEventLevel.Information)) return;
        var logMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message
        };
        var json = JsonConvert.SerializeObject(logMessage, Formatting.Indented);
        logger.Information($"{json}");
    }
}