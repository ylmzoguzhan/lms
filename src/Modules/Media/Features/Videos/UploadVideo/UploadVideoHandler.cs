using Media.Domain.Entities;
using Media.Infrastructure.Data;
using Shared.Abstractions;
using Shared.Contracts.Media;

namespace Media.Features.Videos.UploadVideo;

public record UploadVideoCommand(string Title, string FileName, string ContentType) : ICommand<UploadVideoResponse>;
public record UploadVideoResponse(Guid VideoId, string UploadUrl);
public class UploadVideoHandler(MediaDbContext dbContext, IStorageService storageService, IIntegrationBus eventBus) : ICommandHandler<UploadVideoCommand, UploadVideoResponse>
{
    public async Task<UploadVideoResponse> HandleAsync(UploadVideoCommand command, CancellationToken ct = default)
    {
        var blobPath = $"uploads/{Guid.NewGuid()}_{command.FileName}";
        var video = new Video(command.Title, blobPath);
        dbContext.Videos.Add(video);

        var uploadUrl = await storageService.GetPresignedUrlAsync(blobPath, TimeSpan.FromMinutes(15), ct);

        await eventBus.PublishAsync(new VideoUploadedIntegrationEvent(video.Id, blobPath, command.ContentType), ct);
        await dbContext.SaveChangesAsync(ct);

        return new UploadVideoResponse(video.Id, uploadUrl);
    }
}
