using Microsoft.AspNetCore.Http;
using Shared.Abstractions.Response;

namespace Shared.Infrastructure.Response;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; }

    public static ApiResponse<T> FromResult(Result<T> result)
    {
        return new ApiResponse<T>
        {
            Success = result.IsSuccess,
            Data = result.Data,
            Error = result.ErrorMessage,
            StatusCode = result.StatusCode
        };
    }
}

public class ResultEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result != null && IsResultType(result))
        {
            var wrapperType = typeof(ApiResponse<>).MakeGenericType(result.GetType().GetGenericArguments()[0]);
            var method = wrapperType.GetMethod("FromResult", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            if (method != null)
            {
                var wrappedResult = method.Invoke(null, new[] { result });

                var statusCode = (int)result.GetType().GetProperty("StatusCode")?.GetValue(result);
                var isSuccess = (bool)result.GetType().GetProperty("IsSuccess")?.GetValue(result);

                if (!isSuccess)
                {
                    return statusCode switch
                    {
                        404 => Results.NotFound(wrappedResult),
                        403 => Results.Forbid(),
                        _ => Results.BadRequest(wrappedResult)
                    };
                }

                return statusCode switch
                {
                    201 => Results.Created(string.Empty, wrappedResult),
                    204 => Results.NoContent(),
                    _ => Results.Ok(wrappedResult)
                };
            }
        }

        return result;
    }

    private bool IsResultType(object obj)
    {
        var type = obj.GetType();
        return type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Result<>);
    }
}