using System.Text.Json.Serialization;

namespace Common.Lib.ResponseHandler;

public sealed class ResponseModel
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; set; }
}