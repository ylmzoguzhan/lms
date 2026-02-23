namespace Shared.Abstractions.Messaging.Integration;

public interface IIntegrationConsumer<in TEvent> where TEvent : class
{
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}