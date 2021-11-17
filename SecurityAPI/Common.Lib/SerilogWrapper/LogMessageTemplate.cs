using System.Net;
using Newtonsoft.Json;

namespace Common.Lib.SerilogWrapper;

public class LogMessageTemplate
{
    public DateTime When { get; } = DateTime.Now;
    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.InternalServerError;
    public string MemberName { get; set; }
    public string SourceFilePath { get; set; }
    public int SourceLineNumber { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    public string Classname { get; set; }
}