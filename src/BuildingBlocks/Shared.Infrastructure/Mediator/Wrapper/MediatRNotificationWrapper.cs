using MediatR;
using Shared.Abstractions.Mediator;

namespace Shared.Infrastructure.Mediator.Wrapper;

public record MediatRNotificationWrapper<T>(T Event) : INotification
    where T : IInternalEvent;