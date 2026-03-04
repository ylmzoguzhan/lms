using Shared.Abstractions.Domain;

namespace Media.Domain.Entities;

public class Video : BaseEntity
{
    public string FileName { get; private set; }
    public string BlobPath { get; private set; }
    public string? HlsPath { get; private set; }
    public VideoStatus Status { get; private set; }
    public string ContentType { get; set; }
    public Video(string fileName, string blobPath, string contentType)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
        BlobPath = blobPath;
        Status = VideoStatus.Pending;
        ContentType = contentType;
    }

    public void MarkAsProcessing() => Status = VideoStatus.Processing;
    public void MarkAsReady(string hlsPath)
    {
        Status = VideoStatus.Ready;
        HlsPath = hlsPath;
    }

    public void MarkAsFailed()
    {
        Status = VideoStatus.Failed;
    }
}
public enum VideoStatus
{
    Pending,
    Processing,
    Ready,
    Failed
}