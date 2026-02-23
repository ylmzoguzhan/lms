namespace Shared.Abstractions.Messaging.Internal;

public interface IInternalEventBus
{
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
}