namespace Shared.Abstractions.Mediator;

public interface ICommand<out TResponse> { }
public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken ct = default);
}
