using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class NotFoundException : BaseException
{
    // Without this constructor, deserialization will fail
    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public NotFoundException(string message = "",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.NotFound,
        (message.IsNullOrEmpty() ? ResponseMessage.NotFound.message : message, ResponseMessage.NotFound.type))
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