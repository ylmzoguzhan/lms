using MediatR;

namespace Shared.Infrastructure.Messaging.Internal;

internal record MediatRCommandWrapper<TResponse>(object Command) : IRequest<TResponse>;