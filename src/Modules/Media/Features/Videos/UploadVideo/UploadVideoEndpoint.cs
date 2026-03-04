using Media.Contracts;

namespace Media.Features.Videos.UploadVideo;

public static class UploadVideoEndpoint
{
    public static void MapUploadVideo(this IEndpointRouteBuilder app)
    {
        app.MapPost("/media/videos/upload", async (UploadVideoCommand command, [FromServices] IDispatcher internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return Results.Ok(result);
        }).WithTags("Videos");
    }
}
