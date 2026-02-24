namespace Shared.Abstractions.Messaging.Outbox;

public interface IOutboxProcessor
{
    Task ProcessAsync(CancellationToken ct = default);
}
