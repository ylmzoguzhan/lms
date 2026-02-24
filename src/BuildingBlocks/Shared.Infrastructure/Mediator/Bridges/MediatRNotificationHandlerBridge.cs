using MediatR;
using Shared.Abstractions.Mediator;
using Shared.Infrastructure.Mediator.Wrapper;

namespace Shared.Infrastructure.Mediator.Bridges;

public class MediatRNotificationHandlerBridge<T>(IInternalEventHandler<T> handler)
    : INotificationHandler<MediatRNotificationWrapper<T>>
    where T : IInternalEvent
{
    public async Task Handle(MediatRNotificationWrapper<T> notification, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(notification.Event, cancellationToken);
    }
}