namespace Shared.Abstractions;

public interface ICommand<out TResponse> { }

public interface IEvent { }

public interface IInternalBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : class, IEvent;

    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
}