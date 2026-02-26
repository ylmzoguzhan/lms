using Shared.Abstractions.Domain;

namespace Media.Domain.Entities;

public class Video : BaseEntity
{
    public string Title { get; private set; }
    public string BlobPath { get; private set; }
    public string? HlsPath { get; private set; }
    public VideoStatus Status { get; private set; }

    public Video(string title, string blobPath)
    {
        Id = Guid.NewGuid();
        Title = title;
        BlobPath = blobPath;
        Status = VideoStatus.Pending;
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