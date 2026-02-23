namespace Shared.Abstractions.Messaging;

public interface IIntegrationEventBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}
