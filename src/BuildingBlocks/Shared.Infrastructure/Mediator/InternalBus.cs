using MediatR;
using Shared.Abstractions.Mediator;
using Shared.Infrastructure.Mediator.Wrapper;

namespace Shared.Infrastructure.Mediator;

public class InternalBus(IMediator mediator) : IInternalBus
{
    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
    {
        var wrapper = new MediatRCommandWrapper<TResponse>(command);
        return await mediator.Send(wrapper, ct);
    }
    public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
    {
        var wrapper = new MediatRCommandWrapper<TResponse>(query);
        return await mediator.Send(wrapper, ct);
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : IInternalEvent
    {
        var notification = new MediatRNotificationWrapper<TEvent>(@event);
        await mediator.Publish(notification, ct);
    }
}