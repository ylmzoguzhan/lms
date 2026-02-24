using MediatR;

namespace Shared.Infrastructure.Messaging.Internal;

public class MediatRNotificationWrapper<TEvent>(TEvent @event) : INotification
{
}
