using Shared.Abstractions.Request;

namespace Media.Contracts;

public record ProcessVideoIntegrationEvent(Guid mediaId) : IInternalEvent;