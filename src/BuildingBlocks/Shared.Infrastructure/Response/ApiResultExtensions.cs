using Microsoft.AspNetCore.Http;
using Shared.Abstractions.Response;

namespace Shared.Infrastructure.Response;

public static class ApiResultExtensions
{
    public static IResult ToApi<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
        {
            return result.StatusCode switch
            {
                404 => Results.NotFound(new { error = result.ErrorMessage }),
                403 => Results.Forbid(),
                _ => Results.BadRequest(new { error = result.ErrorMessage })
            };
        }

        return result.StatusCode switch
        {
            201 => Results.Created(string.Empty, result.Data),
            204 => Results.NoContent(),
            _ => Results.Ok(result.Data)
        };
    }
}