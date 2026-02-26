using System.Reflection;
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
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result == null)
            return null;

        if (IsResultType(result))
        {
            var resultType = result.GetType();
            var dataProp = resultType.GetProperty("Data");
            var statusCodeProp = resultType.GetProperty("StatusCode");
            var isSuccessProp = resultType.GetProperty("IsSuccess");

            var data = dataProp?.GetValue(result);
            var statusCode = (int)(statusCodeProp?.GetValue(result) ?? 200);
            var isSuccess = (bool)(isSuccessProp?.GetValue(result) ?? true);

            if (data != null && data.GetType().IsGenericType &&
                data.GetType().GetGenericTypeDefinition() == typeof(PagedList<>))
            {
                if (!isSuccess)
                {
                    return statusCode switch
                    {
                        404 => Results.NotFound(data),
                        403 => Results.Forbid(),
                        _ => Results.BadRequest(data)
                    };
                }

                return statusCode switch
                {
                    201 => Results.Created(string.Empty, data),
                    204 => Results.NoContent(),
                    _ => Results.Ok(data)
                };
            }

            var wrapperType = typeof(ApiResponse<>)
                .MakeGenericType(resultType.GetGenericArguments()[0]);

            var method = wrapperType.GetMethod("FromResult",
                BindingFlags.Static | BindingFlags.Public);

            if (method != null)
            {
                var wrappedResult = method.Invoke(null, new[] { result });

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