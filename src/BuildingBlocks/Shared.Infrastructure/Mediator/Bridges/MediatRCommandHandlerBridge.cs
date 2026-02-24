using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Mediator;
using Shared.Infrastructure.Mediator.Wrapper;

namespace Shared.Infrastructure.Mediator.Bridges;

internal class MediatRCommandHandlerBridge<TRequest, TResponse>(IServiceProvider serviceProvider)
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken ct)
    {
        if (request is MediatRCommandWrapper<TResponse> wrapper)
        {
            var command = wrapper.Command;
            var commandType = command.GetType();

            var handlerType = request switch
            {
                MediatRCommandWrapper<TResponse> w when w.Command is ICommand<TResponse>
                    => typeof(ICommandHandler<,>).MakeGenericType(w.Command.GetType(), typeof(TResponse)),

                MediatRCommandWrapper<TResponse> w when w.Command is IQuery<TResponse>
                    => typeof(IQueryHandler<,>).MakeGenericType(w.Command.GetType(), typeof(TResponse)),

                _ => throw new InvalidOperationException("Bilinmeyen istek tipi!")
            };
            var handler = serviceProvider.GetRequiredService(handlerType);

            var method = handlerType.GetMethod("HandleAsync");
            return await (Task<TResponse>)method!.Invoke(handler, [command, ct])!;
        }
        throw new InvalidOperationException("Komut tipi çözülemedi!");
    }
}
