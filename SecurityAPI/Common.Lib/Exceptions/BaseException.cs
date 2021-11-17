using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.SerilogWrapper;
using Newtonsoft.Json;


namespace Common.Lib.Exceptions;

[Serializable]
public class BaseException : Exception
{
    /// <summary>
    ///     Default constructor
    /// </summary>
    public BaseException()
    {
    }

    // Without this constructor, deserialization will fail
    protected BaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public BaseException(Exception exception, [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(exception.Message, exception)
    {
        exception.ThrowIfHttpException();
        StatusCode = (int) HttpStatusCode.InternalServerError;
        Type = exception.GetBaseException().GetType().ToString();
        LogException(exception, memberName, sourceFilePath, sourceLineNumber);
    }

    public BaseException(Exception exception, string message, [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(message, exception)
    {
        StatusCode = (int) HttpStatusCode.InternalServerError;
        Type = exception.GetBaseException().GetType().ToString();
        LogException(exception, memberName, sourceFilePath, sourceLineNumber);
    }


    public BaseException(Exception exception, (string message, string type) responseMessage,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(
        responseMessage.message.IsNullOrEmpty() ? exception.Message : responseMessage.message, exception)
    {
        //exception.ThrowIfHttpException();
        StatusCode = (int) HttpStatusCode.InternalServerError;
        Type = responseMessage.type.IsNullOrEmpty()
            ? exception.GetBaseException().GetType().ToString()
            : responseMessage.type;
        LogException(exception, memberName, sourceFilePath, sourceLineNumber);
    }

    public BaseException((string message, string type) responseMessage) : base(responseMessage.message)
    {
        StatusCode = HttpStatusCode.InternalServerError.ToInteger();
        Type = responseMessage.type;
        LogException(responseMessage.message, HttpStatusCode.InternalServerError.ToInteger(),
            ResponseMessage.UnknownApiError.message);
    }

    public BaseException(HttpStatusCode httpStatusCode, (string message, string type) responseMessage) : base(
        responseMessage.message)
    {
        StatusCode = (int) httpStatusCode;
        Type = responseMessage.type;
        LoggerWrapper.Error(JsonConvert.SerializeObject(LogMessage, Formatting.Indented));
    }

    public int StatusCode { get; set; }
    public string Type { get; set; }

    public LogMessageTemplate LogMessage { get; set; }


    private void LogException(string errorSource, int errorId, string errorDescription,
        Exception exception = null)
    {
        LoggerWrapper.Error(exception, $"{errorSource} -- Error Id: {errorId}, {errorDescription} ");
    }

    private void LogException(Exception exception,
        string memberName = "",
        string sourceFilePath = "",
        int sourceLineNumber = 0)
    {
        LoggerWrapper.Error(exception, sourceLineNumber, memberName, sourceFilePath);
    }
}