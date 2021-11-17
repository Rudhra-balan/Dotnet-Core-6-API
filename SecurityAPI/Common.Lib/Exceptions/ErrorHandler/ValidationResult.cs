using System.Net;
using Common.Lib.Extenstion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Lib.Exceptions.ErrorHandler;

public class ValidationResult : IActionResult
{
    public Task ExecuteResultAsync(ActionContext context)
    {
        var modelStateEntries = context.ModelState.Where(e => e.Value.Errors.Count > 0).ToArray();
        var errors = new List<ErrorDetails>();
        var error = new ErrorDetails();


        if (modelStateEntries.Any())
        {
            if (modelStateEntries.Length == 1 && modelStateEntries[0].Value.Errors.Count == 1 &&
                modelStateEntries[0].Key == string.Empty)
                error.Description = modelStateEntries[0].Value.Errors[0].ErrorMessage;
            else
                foreach (var modelStateEntry in modelStateEntries)
                    errors.AddRange(modelStateEntry.Value.Errors.Select(modelStateError => new ErrorDetails
                    {
                        Type = HttpStatusCode.BadRequest.ToString(),
                        Description = modelStateError.ErrorMessage
                    }));
        }

        if (!error.Description.IsNullOrEmpty()) errors.Add(error);

        context.HttpContext.Response.Headers.Add(nameof(ValidationResult), "true");
        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
        var errorMessage = new ErrorList {Errors = errors};
        context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
        context.HttpContext.Response.WriteAsync(errorMessage.ToJson());
        return Task.CompletedTask;
    }
}