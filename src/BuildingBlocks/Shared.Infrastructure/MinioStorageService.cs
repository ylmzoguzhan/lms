using Minio;
using Minio.DataModel.Args;
using Shared.Abstractions;

namespace Shared.Infrastructure;

public class MinioStorageService(IMinioClient minioClient) : IStorageService
{
    private const string BucketName = "lms-media";
    public async Task<string> GetPresignedUrlAsync(string fileName, TimeSpan expiry, CancellationToken ct = default)
    {
        var args = new PresignedPutObjectArgs()
            .WithBucket(BucketName)
            .WithObject(fileName)
            .WithExpiry((int)expiry.TotalSeconds);
        return await minioClient.PresignedPutObjectAsync(args);
    }

    public async Task<bool> UploadVideAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
    {
        var args = new PutObjectArgs()
            .WithBucket(BucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);

        await minioClient.PutObjectAsync(args, ct);
        return true;
    }
}
