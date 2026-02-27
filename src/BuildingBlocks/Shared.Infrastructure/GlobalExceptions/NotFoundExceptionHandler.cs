using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Exceptions;
using Shared.Abstractions.Validator;

namespace Shared.Infrastructure.GlobalExceptions;

public class NotFoundExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not NotFoundException notFoundException)
            return false;
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Kayıt bulunamadı",
            Detail = notFoundException.Message,
        };
        await context.Response.WriteAsJsonAsync(problemDetails, ct);
        return true;
    }
}
