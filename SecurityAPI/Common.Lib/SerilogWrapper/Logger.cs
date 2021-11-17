using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace Common.Lib.SerilogWrapper;

public class Logger : Interface.ILogger
{
    private readonly ILogger logger;

    public Logger(ILogger _logger)
    {
        logger = _logger;
    }

    public void Error(string applicationMessage)
    {
        if (!logger.IsEnabled(LogEventLevel.Error)) return;
        logger.Error(applicationMessage);
    }

    public void Error(Exception ex, string applicationMessage)
    {
        if (!logger.IsEnabled(LogEventLevel.Error)) return;
        var exceptionMessage = new ExceptionMessageTemplate(ex)
        {
            ApplicationMessage = applicationMessage
        };
        var json = JsonConvert.SerializeObject(exceptionMessage, Formatting.Indented);
        logger.Error($"{json}", ex);
    }

    public void Error(Exception ex, int sourceLineNumber = 0,
        string memberName = "",
        string sourceFilePath = "")
    {
        if (!logger.IsEnabled(LogEventLevel.Error)) return;
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


    public void Warn(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!logger.IsEnabled(LogEventLevel.Warning)) return;
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

    public void Warn(string message = "Warning",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!logger.IsEnabled(LogEventLevel.Warning)) return;
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

    public void Debug(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!logger.IsEnabled(LogEventLevel.Debug)) return;
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

    public void Debug(string message = "Debug",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!logger.IsEnabled(LogEventLevel.Debug)) return;
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

    public void Info(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!logger.IsEnabled(LogEventLevel.Information)) return;
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

    public void Info(string message = "Information",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!logger.IsEnabled(LogEventLevel.Information)) return;
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