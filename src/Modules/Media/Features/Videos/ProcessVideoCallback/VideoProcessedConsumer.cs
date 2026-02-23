using Media.Infrastructure.Data;
using Shared.Abstractions.Messaging.Integration;
using Shared.Contracts.Media;

namespace Media.Features.Videos.ProcessVideoCallback;

public class VideoProcessedHandler(MediaDbContext dbContext)
    : IIntegrationConsumer<VideoProcessedIntegrationEvent>
{
    public async Task HandleAsync(VideoProcessedIntegrationEvent @event, CancellationToken ct)
    {
        var video = await dbContext.Videos.FindAsync(@event.VideoId);
        if (video == null) return;

        if (@event.Success) video.MarkAsReady(@event.HlsPath!);
        else video.MarkAsFailed();

        await dbContext.SaveChangesAsync(ct);
    }
}