using MediatR;
using Shared.Abstractions.Messaging.Internal;

namespace Shared.Infrastructure.Messaging.Internal;

internal class MediatRInternalCommandBus(IMediator mediator) : IInternalEventBus
{
    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
    {
        var wrapper = new MediatRCommandWrapper<TResponse>(command);
        return await mediator.Send(wrapper, ct);
    }
}