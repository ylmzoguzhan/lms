using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Media.Features.Videos.UploadVideo;

public static class UploadVideoEndpoint
{
    public static void MapUploadVideo(this IEndpointRouteBuilder app)
    {
        app.MapPost("/media/videos/upload", async (UploadVideoCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithTags("Videos");
    }
}
