using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Validator;

namespace Shared.Infrastructure;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not AppValidationException validationEx)
            return false;

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Detail = "Bir veya daha fazla doğrulama hatası oluştu.",
            Extensions = {
                ["errors"] = validationEx.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage))
            }
        };

        await context.Response.WriteAsJsonAsync(problemDetails, ct);
        return true;
    }
}