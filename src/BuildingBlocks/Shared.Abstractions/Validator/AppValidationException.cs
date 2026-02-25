namespace Shared.Abstractions.Validator;

public class AppValidationException(IEnumerable<ValidationError> errors)
    : Exception("Validation failed")
{
    public IEnumerable<ValidationError> Errors { get; } = errors;
}
