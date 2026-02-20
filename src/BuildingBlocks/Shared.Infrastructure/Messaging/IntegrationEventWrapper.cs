using MassTransit;
using Shared.Abstractions.Messaging;

namespace Shared.Infrastructure.Messaging;

public class IntegrationEventWrapper<TEvent, THandler>(THandler handler)
    : IConsumer<TEvent>
    where TEvent : class
    where THandler : IIntegrationEventHandler<TEvent>
{
    public async Task Consume(ConsumeContext<TEvent> context)
    {
        await handler.HandleAsync(context.Message, context.CancellationToken);
    }
}