using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Common.Lib.Extenstion;

namespace Common.Lib.Exceptions.ErrorHandler;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;


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
        context.Result = new BadRequestObjectResult(errors);
    }
}