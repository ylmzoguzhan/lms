using Shared.Abstractions.Mediator;

namespace Identity.Contracts;

public record UserRegisteredEvent(Guid UserId, string Email, string UserName) : IInternalEvent;
