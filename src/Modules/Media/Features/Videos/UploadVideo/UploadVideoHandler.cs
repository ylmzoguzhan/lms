using Media.Domain.Entities;
using Media.Infrastructure.Data;
using MediatR;
using Shared.Abstractions;
using Shared.Contracts.Media;

namespace Media.Features.Videos.UploadVideo;

public record UploadVideoCommand(string Title, string FileName, string ContentType) : IRequest<UploadVideoResponse>;
public record UploadVideoResponse(Guid VideoId, string UploadUrl);
public class UploadVideoHandler(MediaDbContext dbContext, IStorageService storageService, IEventBus eventBus) : IRequestHandler<UploadVideoCommand, UploadVideoResponse>
{
    public async Task<UploadVideoResponse> Handle(UploadVideoCommand request, CancellationToken ct)
    {
        var blobPath = $"uploads/{Guid.NewGuid()}_{request.FileName}";
        var video = new Video(request.Title, blobPath);
        dbContext.Videos.Add(video);

        var uploadUrl = await storageService.GetPresignedUrlAsync(blobPath, TimeSpan.FromMinutes(15), ct);

        await eventBus.PublishAsync(new VideoUploadedIntegrationEvent(video.Id, blobPath, request.ContentType), ct);
        await dbContext.SaveChangesAsync(ct);

        return new UploadVideoResponse(video.Id, uploadUrl);
    }
}
