using MassTransit;
using Shared.Abstractions;

namespace Shared.Infrastructure;

public class MassTransitEventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    public async Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
    {
        await publishEndpoint.Publish(message);
    }
}
