using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Exceptions.ErrorHandler;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class ValidationException : BaseException
{
    // Without this constructor, deserialization will fail
    protected ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public ValidationException(List<string> message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.BadRequest,
        (message.ToValidationMessage().IsNullOrEmpty() ? ResponseMessage.BadRequest.message : message.ToValidationMessage(),
            nameof(ValidationException)))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message.ToValidationMessage(),
            HttpStatusCode = HttpStatusCode.BadRequest
        };
    }

    public ValidationException(List<(string errorMessage, string errorType)> message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.BadRequest,
        (message.ToValidationMessage().IsNullOrEmpty() ? ResponseMessage.BadRequest.message : message.ToValidationMessage(),
            nameof(ValidationException)))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message.ToValidationMessage(),
            HttpStatusCode = HttpStatusCode.BadRequest
        };
    }

    public ValidationException(List<ErrorDetails> message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.BadRequest,
        (message.ToValidationMessage().IsNullOrEmpty() ? ResponseMessage.BadRequest.message : message.ToValidationMessage(),
            nameof(ValidationException)))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message.ToValidationMessage(),
            HttpStatusCode = HttpStatusCode.BadRequest
        };
    }
}

public static class ValidationExceptionExtenstion
{
    public static string ToValidationMessage(this List<string> messages)
    {
        if (!messages.Any()) return string.Empty;
        var errorDetails = messages.Select(message => new ErrorDetails
        {
            Type = HttpStatusCode.BadRequest.ToString(),
            Description = message
        }).ToList();
        var fullResponse = new ErrorList() {Errors = errorDetails};
        return messages.Count == 0 ? string.Empty : fullResponse.ToJson();
    }

    public static string ToValidationMessage(this List<(string errorMessage, string errorType)> messages)
    {
        if (!messages.Any()) return string.Empty;
        var errorDetails = messages.Select(message => new ErrorDetails
        {
            Type = message.errorType,
            Description = message.errorMessage
        }).ToList();
        var fullResponse = new ErrorList() {Errors = errorDetails};
        return messages.Count == 0 ? string.Empty : fullResponse.ToJson();
    }

    public static string ToValidationMessage(this List<ErrorDetails> messages)
    {
        if (!messages.Any()) return string.Empty;

        var fullResponse = new ErrorList() {Errors = messages};
        return messages.Count == 0 ? string.Empty : fullResponse.ToJson();
    }
}