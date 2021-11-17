using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class NotModifiedException : BaseException
{
    // Without this constructor, deserialization will fail
    protected NotModifiedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public NotModifiedException(string message = "",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.NotModified,
        (message.IsNullOrEmpty() ? ResponseMessage.NotModified.message : message, ResponseMessage.NotModified.type))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message,
            HttpStatusCode = HttpStatusCode.NotFound
        };
    }
}