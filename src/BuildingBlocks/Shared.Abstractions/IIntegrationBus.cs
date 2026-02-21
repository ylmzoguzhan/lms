namespace Shared.Abstractions;

public interface IIntegrationBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}
