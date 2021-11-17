using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Common.Lib.Exceptions;
using Common.Lib.ResponseHandler.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NotImplementedException = Common.Lib.Exceptions.NotImplementedException;

namespace Common.Lib.Extenstion;

public static class SystemExtensions
{
    public static bool IsNullOrEmpty(this string? value)
    {
        return value == null || value.Trim().Length == 0;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? value)
    {
        return value == null || !value.Any();
    }

    public static bool IsNullOrEmpty<T, TK>(this IDictionary<T, TK>? value)
    {
        return value == null || value.Count == 0;
    }

    public static bool IsAnyNullOrEmpty(this object myObject)
    {
        return (from pi in myObject.GetType().GetProperties()
            where pi.PropertyType == typeof(string)
            select (string) pi.GetValue(myObject)).Any(string.IsNullOrEmpty);
    }

    public static int ToInteger<T>(this T e) where T : IConvertible
    {
        var value = 0;

        if (e is not Enum) return value;
        var type = e.GetType();
        var values = Enum.GetValues(type);

        return values.Cast<int>().FirstOrDefault(val => val == e.ToInt32(CultureInfo.InvariantCulture));
    }

    public static uint TouInt<T>(this T e) where T : IConvertible
    {
        uint value = 0;

        if (e is not Enum) return value;
        var type = e.GetType();
        var values = Enum.GetValues(type);

        return values.Cast<uint>().FirstOrDefault(val => val == e.ToInt32(CultureInfo.InvariantCulture));
    }

    public static int ToInt(this string number)
    {
        return int.TryParse(number, out var parsedInt) ? parsedInt : 0;
    }

    public static Guid ToGuid(this string value)
    {
        return Guid.TryParse(value, out var parsedGuid) ? parsedGuid : default;
    }


    public static decimal ToDecimal(this string number)
    {
        return decimal.TryParse(number, out var parsed) ? parsed : 0;
    }

    public static float ToFloat(this string number)
    {
        return float.TryParse(number, out var parsed) ? parsed : 0;
    }

    public static float ToFloat(this decimal number)
    {
        return (float) number;
    }

    public static void ThrowIfHttpException(this Exception exception)
    {
        if (exception is UnAuthorizedException or NoContentException or BadRequestException
            or NotFoundException or NotImplementedException or ForbiddenException or InternalServerErrorException
            or ValidationException or ConflictException)
            throw exception;
    }

    public static string ToJson(this object rawJson)
    {
        return JsonConvert.SerializeObject(rawJson, JsonSettings());
    }

    public static string ToCamelCase(this string value)
    {
        var pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
        return new string(
            new CultureInfo("en-US", false)
                .TextInfo
                .ToTitleCase(
                    string.Join(" ", pattern.Matches(value)).ToLower()
                )
                .Replace(@" ", "")
                .Select((x, i) => i == 0 ? char.ToLower(x) : x)
                .ToArray()
        );
    }

    public static string GetExceptionType(this Exception exception)
    {
        return exception switch
        {
            UnAuthorizedException => HttpStatusCode.Unauthorized.ToString(),
            NoContentException => HttpStatusCode.NoContent.ToString(),
            ValidationException => HttpStatusCode.BadRequest.ToString(),
            BadRequestException => HttpStatusCode.BadRequest.ToString(),
            NotFoundException => HttpStatusCode.BadRequest.ToString(),
            NotImplementedException => HttpStatusCode.NotImplemented.ToString(),
            ForbiddenException => HttpStatusCode.Forbidden.ToString(),
            ConflictException => HttpStatusCode.Conflict.ToString(),
            _ => HttpStatusCode.InternalServerError.ToString()
        };
    }

    public static (string message, string type) GetExceptionMessage(this Exception exception)
    {
        return exception.GetBaseException() switch
        {
            UnAuthorizedException => (ResponseMessage.UnAuthorized.message, ResponseMessage.UnAuthorized.type),
            NoContentException => (ResponseMessage.NoContent.message, ResponseMessage.UnAuthorized.type),
            BadRequestException => (ResponseMessage.BadRequest.message, ResponseMessage.UnAuthorized.type),
            NotFoundException => (ResponseMessage.NotFound.message, ResponseMessage.UnAuthorized.type),
            NotImplementedException => (ResponseMessage.NotImplemented.message, ResponseMessage.UnAuthorized.type),
            ForbiddenException => (ResponseMessage.Forbidden.message, ResponseMessage.UnAuthorized.type),
            ConflictException => (ResponseMessage.Conflict.message, ResponseMessage.UnAuthorized.type),
            IndexOutOfRangeException => (ResponseMessage.IndexOutOfRangeException.message,
                ResponseMessage.UnAuthorized.type),
            NullReferenceException => (ResponseMessage.NullReferenceException.message,
                ResponseMessage.UnAuthorized.type),
            AccessViolationException => (ResponseMessage.AccessViolationException.message,
                ResponseMessage.AccessViolationException.type),
            ObjectDisposedException => (ResponseMessage.ObjectDisposedException.message,
                ResponseMessage.ObjectDisposedException.type),
            UriFormatException => (ResponseMessage.UriFormatException.message, ResponseMessage.UriFormatException.type),
            PathTooLongException => (ResponseMessage.PathTooLongException.message,
                ResponseMessage.PathTooLongException.type),
            InvalidOperationException => (ResponseMessage.InvalidOperationException.message,
                ResponseMessage.InvalidOperationException.type),
            ArgumentNullException => (ResponseMessage.ArgumentNullException.message,
                ResponseMessage.ArgumentNullException.type),
            ArgumentOutOfRangeException => (ResponseMessage.ArgumentOutOfRangeException.message,
                ResponseMessage.ArgumentOutOfRangeException.type),
            ArgumentException => (ResponseMessage.ArgumentException.message, ResponseMessage.ArgumentException.type),
            UnauthorizedAccessException => (ResponseMessage.UnauthorizedAccessException.message,
                ResponseMessage.UnAuthorized.type),
            DirectoryNotFoundException directoryNotFoundException => (
                ResponseMessage.DirectoryNotFoundException.message, ResponseMessage.DirectoryNotFoundException.type),
            FileNotFoundException => (ResponseMessage.FileNotFoundException.message,
                ResponseMessage.FileNotFoundException.type),
            IOException ioException => (
                (ioException.HResult & 0x0000FFFF) == 32
                    ? ResponseMessage.FileSharingViolation.message
                    : ResponseMessage.IoException.message, nameof(IOException)),
            DivideByZeroException => (ResponseMessage.DivideByZeroException.message,
                ResponseMessage.DivideByZeroException.type),
            FormatException => (ResponseMessage.FormatException.message, ResponseMessage.FormatException.type),
            KeyNotFoundException => (ResponseMessage.KeyNotFoundException.message,
                ResponseMessage.KeyNotFoundException.type),
            Exceptions.NotSupportedException => (ResponseMessage.NotSupportedException.message,
                ResponseMessage.NotSupportedException.type),
            OverflowException => (ResponseMessage.OverflowException.message, ResponseMessage.OverflowException.type),
            OutOfMemoryException => (ResponseMessage.OutOfMemoryException.message,
                ResponseMessage.OutOfMemoryException.type),
            StackOverflowException => (ResponseMessage.StackOverflowException.message,
                ResponseMessage.StackOverflowException.type),
            TimeoutException => (ResponseMessage.TimeoutException.message, ResponseMessage.TimeoutException.type),
            _ => (ResponseMessage.UnknownApiError.message, exception.GetBaseException().GetType().ToString())
        };
    }

    private static JsonSerializerSettings JsonSettings()
    {
        return new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.None,
            Culture = CultureInfo.CurrentUICulture
        };
    }
}