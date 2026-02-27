namespace Shared.Abstractions.Exceptions;

public class NotFoundException(string message) : Exception($"{message} Not found")
{
    public string Message { get; } = message;
}
