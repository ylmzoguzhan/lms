
using Media.Contracts;

namespace Media.Features.Videos.ProcessVideo;

public class ProcessVideoHandler(MediaDbContext dbContext, IIntegrationEventBus eventBus) : IInternalEventHandler<ProcessVideoIntegrationEvent>
{
    public async Task HandleAsync(ProcessVideoIntegrationEvent @event, CancellationToken ct = default)
    {
        var video = await dbContext.Videos.FindAsync(@event.mediaId);
        await Task.Delay(10000);
        await eventBus.PublishAsync(new VideoUploadedIntegrationEvent(video.Id, video.BlobPath, "mp4"), ct);
    }
}
