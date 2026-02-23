namespace Shared.Abstractions.Messaging.Integration;

public interface IIntegrationEventBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}
