namespace Shared.Abstractions;

public interface IStorageService
{
    Task<string> GetPresignedUrlAsync(string fileName, TimeSpan expiry, CancellationToken ct = default);
    Task<bool> UploadVideAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default);
}
