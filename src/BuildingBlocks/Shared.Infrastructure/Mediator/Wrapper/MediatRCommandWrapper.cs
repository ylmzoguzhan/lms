using MediatR;

namespace Shared.Infrastructure.Mediator.Wrapper;

internal record MediatRCommandWrapper<TResponse>(object Command) : IRequest<TResponse>;