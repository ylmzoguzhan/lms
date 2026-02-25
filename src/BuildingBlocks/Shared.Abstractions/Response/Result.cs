namespace Shared.Abstractions.Response;

public record Result<T>(T? Data, bool IsSuccess, string? ErrorMessage = null, int StatusCode = 200);