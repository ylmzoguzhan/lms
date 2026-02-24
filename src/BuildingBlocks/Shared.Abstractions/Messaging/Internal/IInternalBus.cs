namespace Shared.Abstractions.Messaging.Internal;

public interface IInternalBus
{
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
    Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : IInternalEvent;
}