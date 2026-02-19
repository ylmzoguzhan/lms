namespace Media.Domain.Entities;

public class Video
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string BlobPath { get; private set; }
    public VideoStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Video(string title, string blobPath)
    {
        Id = Guid.NewGuid();
        Title = title;
        BlobPath = blobPath;
        Status = VideoStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsProcessing() => Status = VideoStatus.Processing;
    public void MarkAsReady() => Status = VideoStatus.Ready;
}
public enum VideoStatus
{
    Pending,
    Processing,
    Ready,
    Failled
}