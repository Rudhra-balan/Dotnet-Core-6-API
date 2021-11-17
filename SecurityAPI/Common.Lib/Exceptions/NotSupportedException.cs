using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class NotSupportedException : BaseException
{
    // Without this constructor, deserialization will fail
    protected NotSupportedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public NotSupportedException((string message, string type) errorMessage,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.BadRequest,
        (errorMessage.message.IsNullOrEmpty() ? ResponseMessage.NotImplemented.message : errorMessage.message,
            nameof(NotSupportedException)))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = errorMessage.message,
            HttpStatusCode = HttpStatusCode.BadRequest
        };
    }
}