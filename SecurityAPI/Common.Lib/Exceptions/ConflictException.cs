using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class ConflictException : BaseException
{
    // Without this constructor, deserialization will fail
    protected ConflictException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public ConflictException(string message = "",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.Conflict,
        (message.IsNullOrEmpty() ? ResponseMessage.Conflict.message : message, ResponseMessage.Conflict.type))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = message,
            HttpStatusCode = HttpStatusCode.Conflict
        };
    }
}