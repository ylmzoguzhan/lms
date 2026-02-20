namespace Shared.Abstractions.Messaging;

public interface IIntegrationEventHandler<in TEvent> where TEvent : class
{
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}