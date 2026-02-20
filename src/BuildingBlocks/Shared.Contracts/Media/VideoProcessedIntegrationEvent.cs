namespace Shared.Contracts.Media;

public record VideoProcessedIntegrationEvent(
    Guid VideoId,
    string HlsPath,
    bool Success);