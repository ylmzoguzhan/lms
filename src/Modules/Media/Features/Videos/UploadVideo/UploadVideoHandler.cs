
using Media.Contracts;
using Shared.Abstractions.Auth;

namespace Media.Features.Videos.UploadVideo;

// public record UploadVideoCommand(string Title, string FileName, string ContentType) : ICommand<UploadVideoResponse>;
// public record UploadVideoResponse(Guid VideoId, string UploadUrl);
public class UploadVideoHandler(MediaDbContext dbContext, IStorageService storageService, IIntegrationEventBus eventBus, IUserService userService) : ICommandHandler<UploadVideoCommand, UploadVideoResponse>
{
    public async Task<UploadVideoResponse> HandleAsync(UploadVideoCommand command, CancellationToken ct = default)
    {
        var blobPath = $"uploads/{userService.UserId}/{Guid.NewGuid()}_{command.FileName}";
        var video = new Video(command.FileName, blobPath, command.ContentType);
        dbContext.Videos.Add(video);
        var uploadUrl = await storageService.GetPresignedUrlAsync(blobPath, TimeSpan.FromMinutes(15), ct);
        await dbContext.SaveChangesAsync(ct);

        return new UploadVideoResponse(video.Id, uploadUrl);
    }
}
