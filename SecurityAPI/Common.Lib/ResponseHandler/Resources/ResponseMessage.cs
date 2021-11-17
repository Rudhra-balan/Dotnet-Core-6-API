using Microsoft.Extensions.Localization;

namespace Common.Lib.ResponseHandler.Resources;

public class ResponseMessage
{
    private static IStringLocalizer localizer = ServiceActivator.GetResource();


    #region [Response/Error Message]

    public static (LocalizedString message, string type) UnAuthorized =>
        (localizer[nameof(UnAuthorized)], nameof(UnAuthorized));

    public static (LocalizedString message, string type) RequestIDConflictException =>
        (localizer[nameof(RequestIDConflictException)], nameof(RequestIDConflictException));

    public static (LocalizedString message, string type) TokenExpired =>
        (localizer[nameof(TokenExpired)], nameof(TokenExpired));

    public static (LocalizedString message, string type) UnknownApiError =>
        (localizer[nameof(UnknownApiError)], nameof(UnknownApiError));

    public static (LocalizedString message, string type) NotFound => (localizer[nameof(NotFound)], nameof(NotFound));
    public static (LocalizedString message, string type) NoContent => (localizer[nameof(NoContent)], nameof(NoContent));

    public static (LocalizedString message, string type) BadRequest =>
        (localizer[nameof(BadRequest)], nameof(BadRequest));

    public static (LocalizedString message, string type) Conflict => (localizer[nameof(Conflict)], nameof(Conflict));

    public static (LocalizedString message, string type) RequestUriTooLong =>
        (localizer[nameof(RequestUriTooLong)], nameof(RequestUriTooLong));

    public static (LocalizedString message, string type) RequiredFieldEmptyException =>
        (localizer[nameof(RequiredFieldEmptyException)], nameof(RequiredFieldEmptyException));

    public static (LocalizedString message, string type) RequiredFieldInvalidException =>
        (localizer[nameof(RequiredFieldInvalidException)], nameof(RequiredFieldInvalidException));

    public static (LocalizedString message, string type) RequiredFieldMissingException =>
        (localizer[nameof(RequiredFieldMissingException)], nameof(RequiredFieldMissingException));

    public static (LocalizedString message, string type) RequiredNonEmptyRequestBodyException =>
        (localizer[nameof(RequiredNonEmptyRequestBodyException)], nameof(RequiredNonEmptyRequestBodyException));

    public static (LocalizedString message, string type) UnsupportedMediaType =>
        (localizer[nameof(UnsupportedMediaType)], nameof(UnsupportedMediaType));

    public static (LocalizedString message, string type) NotImplemented =>
        (localizer[nameof(NotImplemented)], nameof(NotImplemented));

    public static (LocalizedString message, string type) NotModified =>
        (localizer[nameof(NotModified)], nameof(NotModified));

    public static (LocalizedString message, string type) Forbidden => (localizer[nameof(Forbidden)], nameof(Forbidden));

    public static (LocalizedString message, string type) ArgumentNull =>
        (localizer[nameof(ArgumentNull)], nameof(ArgumentNull));

    public static (LocalizedString message, string type) IoException =>
        (localizer[nameof(IoException)], nameof(IoException));

    public static (LocalizedString message, string type) IndexOutOfRangeException => (
        localizer[nameof(IndexOutOfRangeException)], nameof(IndexOutOfRangeException));

    public static (LocalizedString message, string type) NullReferenceException =>
        (localizer[nameof(NullReferenceException)], nameof(NullReferenceException));

    public static (LocalizedString message, string type) InvalidOperationException => (
        localizer[nameof(InvalidOperationException)], nameof(InvalidOperationException));

    public static (LocalizedString message, string type) ArgumentException =>
        (localizer[nameof(ArgumentException)], nameof(ArgumentException));

    public static (LocalizedString message, string type) ArgumentNullException =>
        (localizer[nameof(ArgumentNullException)], nameof(ArgumentNullException));

    public static (LocalizedString message, string type) ArgumentOutOfRangeException => (
        localizer[nameof(ArgumentOutOfRangeException)], nameof(ArgumentOutOfRangeException));

    public static (LocalizedString message, string type) DivideByZeroException =>
        (localizer[nameof(DivideByZeroException)], nameof(DivideByZeroException));

    public static (LocalizedString message, string type) FileNotFoundException =>
        (localizer[nameof(FileNotFoundException)], nameof(FileNotFoundException));

    public static (LocalizedString message, string type) FormatException =>
        (localizer[nameof(FormatException)], nameof(FormatException));

    public static (LocalizedString message, string type) KeyNotFoundException =>
        (localizer[nameof(KeyNotFoundException)], nameof(KeyNotFoundException));

    public static (LocalizedString message, string type) NotSupportedException =>
        (localizer[nameof(NotSupportedException)], nameof(NotSupportedException));

    public static (LocalizedString message, string type) OverflowException =>
        (localizer[nameof(OverflowException)], nameof(OverflowException));

    public static (LocalizedString message, string type) OutOfMemoryException =>
        (localizer[nameof(OutOfMemoryException)], nameof(OutOfMemoryException));

    public static (LocalizedString message, string type) StackOverflowException =>
        (localizer[nameof(StackOverflowException)], nameof(StackOverflowException));

    public static (LocalizedString message, string type) TimeoutException =>
        (localizer[nameof(TimeoutException)], nameof(TimeoutException));

    public static (LocalizedString message, string type) ObjectDisposedException => (
        localizer[nameof(ObjectDisposedException)],
        nameof(ObjectDisposedException));

    public static (LocalizedString message, string type) PathTooLongException =>
        (localizer[nameof(PathTooLongException)], nameof(PathTooLongException));

    public static (LocalizedString message, string type) UriFormatException =>
        (localizer[nameof(UriFormatException)], nameof(UriFormatException));

    public static (LocalizedString message, string type) AccessViolationException => (
        localizer[nameof(AccessViolationException)], nameof(AccessViolationException));

    public static (LocalizedString message, string type) UnauthorizedAccessException => (
        localizer[nameof(UnauthorizedAccessException)], nameof(UnauthorizedAccessException));

    public static (LocalizedString message, string type) FileSharingViolation =>
        (localizer[nameof(FileSharingViolation)], nameof(FileSharingViolation));

    public static (LocalizedString message, string type) DirectoryNotFoundException => (
        localizer[nameof(DirectoryNotFoundException)], nameof(DirectoryNotFoundException));

    public static (LocalizedString message, string type) InvalidCredentials =>
        (localizer[nameof(InvalidCredentials)], nameof(InvalidCredentials));

    public static (LocalizedString message, string type) TokenGeneratedException => (
        localizer[nameof(TokenGeneratedException)],
        nameof(TokenGeneratedException));

    public static (LocalizedString, string) InvalidRefreshToken =>
        (localizer[nameof(InvalidRefreshToken)], nameof(InvalidRefreshToken));

    public static (LocalizedString message, string type) UserNotExistException =>
        (localizer[nameof(UserNotExistException)], nameof(UserNotExistException));

    public static (LocalizedString message, string type) InvalidPasswordException => (
        localizer[nameof(InvalidPasswordException)], nameof(InvalidPasswordException));


    public static (LocalizedString message, string type) InvalidRequestFormatException => (
        localizer[nameof(InvalidRequestFormatException)], nameof(InvalidRequestFormatException));

    #endregion
}