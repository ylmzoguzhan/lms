namespace Shared.Abstractions.Validator;

public record ValidationError(string PropertyName, string ErrorMessage);

public interface IAppValidator<in TRequest>
{
    Task<IEnumerable<ValidationError>> ValidateAsync(TRequest request, CancellationToken ct);
}