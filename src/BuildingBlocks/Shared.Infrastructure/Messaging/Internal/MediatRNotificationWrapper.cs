using MediatR;
using Shared.Abstractions.Messaging.Internal;

namespace Shared.Infrastructure.Messaging.Internal;

public record MediatRNotificationWrapper<T>(T Event) : INotification
    where T : IInternalEvent;