using MassTransit;
using Shared.Abstractions.Messaging;

namespace Shared.Infrastructure.Messaging.Integration;

public class MassTransitConsumerWrapper<TEvent, THandler>(THandler handler)
    : IConsumer<TEvent>
        where TEvent : class
        where THandler : IIntegrationConsumer<TEvent>
{
    public async Task Consume(ConsumeContext<TEvent> context)
    {
        await handler.HandleAsync(context.Message, context.CancellationToken);
    }
}
