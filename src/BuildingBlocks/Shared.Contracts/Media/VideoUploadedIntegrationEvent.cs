namespace Shared.Contracts.Media;

public record VideoUploadedIntegrationEvent(
    Guid VideoId,
    string BlobPath,
    string ContentType);