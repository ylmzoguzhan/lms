using MediatR;
using Shared.Abstractions;

namespace Shared.Infrastructure;

public class MediatRInternalBus(IMediator mediator) : IInternalBus
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : class, IEvent
    {
        await mediator.Publish(@event, ct);
    }

    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
    {
        var result = await mediator.Send(command, ct);

        return (TResponse)result!;
    }
}