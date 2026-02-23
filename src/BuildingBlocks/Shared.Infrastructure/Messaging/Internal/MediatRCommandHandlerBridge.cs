using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Messaging.Internal;
namespace Shared.Infrastructure.Messaging.Internal;

internal class MediatRCommandHandlerBridge<TRequest, TResponse>(IServiceProvider serviceProvider)
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken ct)
    {
        if (request is MediatRCommandWrapper<TResponse> wrapper)
        {
            var command = wrapper.Command;
            var commandType = command.GetType();

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));
            var handler = serviceProvider.GetRequiredService(handlerType);

            var method = handlerType.GetMethod("HandleAsync");
            return await (Task<TResponse>)method!.Invoke(handler, [command, ct])!;
        }
        throw new InvalidOperationException("Komut tipi çözülemedi!");
    }
}
