using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class NoContentException : BaseException
{
    // Without this constructor, deserialization will fail
    protected NoContentException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public NoContentException((string message, string type) errorMessage,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.NoContent,
        (errorMessage.message.IsNullOrEmpty() ? ResponseMessage.NoContent.message : errorMessage.message,
            errorMessage.type.IsNullOrEmpty() ? ResponseMessage.NoContent.type : errorMessage.type))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = errorMessage.message,
            HttpStatusCode = HttpStatusCode.NoContent
        };
    }
}