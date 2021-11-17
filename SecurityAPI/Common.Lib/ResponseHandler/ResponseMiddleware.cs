using System.Net;
using Common.Lib.Exceptions;
using Common.Lib.Exceptions.ErrorHandler;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Common.Lib.ResponseHandler;

public class ResponseMiddleware
{
    #region Private Variable

    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    #endregion

    #region Constructor

    public ResponseMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _environment = env;
    }

    #endregion

    #region Public Member

    public async Task Invoke(HttpContext context)
    {
        if (IsSwagger(context))
        {
            await _next(context);
        }
        else
        {
            var originalBodyStream = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                if (context.Response.StatusCode == (int) HttpStatusCode.Unauthorized) return;

                await _next.Invoke(context);

                bool.TryParse(context.Response.Headers[nameof(ValidationResult)], out var isValidation);
                if (context.Response.StatusCode != (int) HttpStatusCode.OK && !isValidation)
                    await HandleNotSuccessRequestAsync(context, context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    #endregion

    #region Private Member

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ErrorList fullResponse;
        string exceptionType;
        context.Response.ContentType = "text/plain";
        switch (exception.GetBaseException())
        {
            case BaseException ex when ex is ValidationException:
                context.Response.StatusCode = ex.StatusCode;
                context.Response.Body.SetLength(0);
                return context.Response.WriteAsync(ex.Message);
            case BaseException ex
                when ex is ArgumentNullOrEmptyException
                     || ex is BadRequestException
                     || ex is ConflictException
                     || ex is ForbiddenException
                     || ex is InternalServerErrorException
                     || ex is NoContentException
                     || ex is NotFoundException
                     || ex is Exceptions.NotImplementedException
                     || ex is NotModifiedException
                     || ex is UnAuthorizedException:

                context.Response.StatusCode = ex.StatusCode;
                exceptionType = ex.Type;
                fullResponse = ResponseGenerator.ErrorResponseMessage(exceptionType, ex.Message);
                break;
            case BaseException ex:
                context.Response.StatusCode = ex.StatusCode;
                exceptionType = ex.Type;
                fullResponse = ResponseGenerator.ErrorResponseMessage(exceptionType, ex.Message);
                break;

            default:
            {
                (string exceptionMessage, string exceptionInfoType) = _environment.IsDevelopment() == false
                    ? exception.GetExceptionMessage()
                    : ($"Message: {exception.GetBaseException().Message} \n Stack Trace :{exception.StackTrace}",
                        exception.GetBaseException().GetType().ToString());

                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                fullResponse =
                    ResponseGenerator.ErrorResponseMessage(exceptionMessage, exceptionInfoType);
                break;
            }
        }


        var json = fullResponse.ToJson();
        context.Response.Body.SetLength(0);
        return context.Response.WriteAsync(json);
    }

    private Task HandleNotSuccessRequestAsync(HttpContext context, int code, string errorMessage = "")
    {
        context.Response.ContentType = "text/plain";
        var (errorDescription, errorType) = GetErrorMessageByStatusCode(code, errorMessage);
        var apiResponse = new List<ErrorDetails>
        {
            new()
            {
                Type = errorType,
                Description = errorDescription
            }
        };
        var errors = new ErrorList {Errors = apiResponse};
        context.Response.StatusCode = code;
        var json = errors.ToJson();
        context.Response.Body.SetLength(0);
        return context.Response.WriteAsync(json);
    }

    private bool IsSwagger(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/swagger");
    }

    private (string, string) GetErrorMessageByStatusCode(int code, string errorMessage = "")
    {
        return code switch
        {
            (int) HttpStatusCode.NotFound => (ResponseMessage.NotFound.message, ResponseMessage.NotFound.type),
            (int) HttpStatusCode.NoContent => (ResponseMessage.NoContent.message, ResponseMessage.NoContent.type),
            (int) HttpStatusCode.BadRequest => errorMessage.IsNullOrEmpty()
                ? (ResponseMessage.BadRequest.message, ResponseMessage.BadRequest.type)
                : (errorMessage, ResponseMessage.BadRequest.type),
            (int) HttpStatusCode.Forbidden => (ResponseMessage.Forbidden.message, ResponseMessage.Forbidden.type),
            (int) HttpStatusCode.NotModified => (ResponseMessage.NotModified.message, ResponseMessage.NotModified.type),
            (int) HttpStatusCode.NotImplemented => (ResponseMessage.NotImplemented.message,
                ResponseMessage.NotImplemented.type),
            (int) HttpStatusCode.Unauthorized => (ResponseMessage.UnAuthorized.message,
                ResponseMessage.UnAuthorized.type),
            (int) HttpStatusCode.ServiceUnavailable => (ResponseMessage.TimeoutException.message,
                ResponseMessage.TimeoutException.type),
            (int) HttpStatusCode.RequestUriTooLong =>
                (ResponseMessage.RequestUriTooLong.message, ResponseMessage.RequestUriTooLong.type),
            (int) HttpStatusCode.UnsupportedMediaType => (ResponseMessage.UnsupportedMediaType.message,
                ResponseMessage.UnsupportedMediaType.type),
            _ => (ResponseMessage.UnknownApiError.message, HttpStatusCode.InternalServerError.ToString())
        };
    }

    #endregion
}