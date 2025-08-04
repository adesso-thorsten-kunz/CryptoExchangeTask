using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CryptoExchangeTask.API.ExceptionHandler;

public class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger;

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        _logger.LogError(validationException, "Validation failed");
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Extensions = new Dictionary<string, object?>
            {
                ["errors"] = validationException.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                })
            }
        };
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }
}