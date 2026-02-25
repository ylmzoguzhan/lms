using MediatR;

namespace Shared.Infrastructure.Mediator.Wrapper;

public record MediatRCommandWrapper<TResponse>(object Command) : IRequest<TResponse>;