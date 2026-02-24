using MediatR;
using Shared.Abstractions.Messaging.Internal;

namespace Shared.Infrastructure.Messaging.Internal;

public class MediatRNotificationHandlerBridge<T>(IInternalEventHandler<T> handler)
    : INotificationHandler<MediatRNotificationWrapper<T>>
    where T : IInternalEvent
{
    public async Task Handle(MediatRNotificationWrapper<T> notification, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(notification.Event, cancellationToken);
    }
}