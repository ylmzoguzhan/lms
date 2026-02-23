using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions;
using Shared.Abstractions.Messaging.Internal;

namespace Media.Features.Videos.UploadVideo;

public static class UploadVideoEndpoint
{
    public static void MapUploadVideo(this IEndpointRouteBuilder app)
    {
        app.MapPost("/media/videos/upload", async (UploadVideoCommand command, [FromServices] IInternalEventBus internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return Results.Ok(result);
        }).WithTags("Videos");
    }
}
