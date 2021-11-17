using System.Runtime.CompilerServices;

namespace Common.Lib.SerilogWrapper.Interface;

public interface ILogger
{
    void Error(string errorMessage);
    void Error(Exception ex, string errorMessage);

    void Error(Exception ex, int sourceLineNumber = 0,
        string memberName = "",
        string sourceFilePath = "");

    void Warn(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void Warn(string message = "Warning",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void Debug(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void Debug(string message = "Debug",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void Info(Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void Info(string message = "Information",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);
}