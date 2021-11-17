using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class InternalServerErrorException : BaseException
{
    // Without this constructor, deserialization will fail
    protected InternalServerErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public InternalServerErrorException((string message, string type) errorMessage,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(HttpStatusCode.InternalServerError,
        (errorMessage.message.IsNullOrEmpty() ? ResponseMessage.UnknownApiError.message : errorMessage.message,
            errorMessage.type.IsNullOrEmpty() ? ResponseMessage.UnknownApiError.type : errorMessage.type))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = errorMessage.message,
            HttpStatusCode = HttpStatusCode.InternalServerError
        };
    }
}