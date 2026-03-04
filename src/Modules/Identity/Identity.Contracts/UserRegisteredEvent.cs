using Shared.Abstractions.Request;

namespace Identity.Contracts;

public record UserRegisteredEvent(Guid UserId, string Email, string UserName) : IInternalEvent;
