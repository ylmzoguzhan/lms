using MassTransit;
using Shared.Abstractions.Messaging;

namespace Shared.Infrastructure.Messaging.MassTransit;

public class MassTransitEventBus(IPublishEndpoint publishEndpoint) : IIntegrationEventBus
{
    public async Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
    {
        await publishEndpoint.Publish(message, ct);
    }
}
