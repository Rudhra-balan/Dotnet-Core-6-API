using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.SerilogWrapper;

namespace Common.Lib.Exceptions;

[Serializable]
public class ArgumentNullOrEmptyException : BaseException
{
    #region Properties And Fields

    #endregion

    #region Construction and Destruction

    // Without this constructor, deserialization will fail
    protected ArgumentNullOrEmptyException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public ArgumentNullOrEmptyException(string parameterName = "",
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0) : base(statusCode,
        (parameterName.IsNullOrEmpty() ? ResponseMessage.ArgumentNullException.message : string.Format(ResponseMessage.ArgumentNull.message, parameterName),
            ResponseMessage.ArgumentNullException.type))
    {
        LogMessage = new LogMessageTemplate
        {
            MemberName = memberName,
            Classname = Path.GetFileNameWithoutExtension(sourceFilePath),
            SourceLineNumber = sourceLineNumber,
            SourceFilePath = sourceFilePath,
            Message = $"A {parameterName} cannot be null or empty ",
            HttpStatusCode = statusCode
        };
    }

    #endregion
}