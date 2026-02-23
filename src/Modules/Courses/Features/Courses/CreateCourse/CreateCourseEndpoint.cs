using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.Messaging.Internal;

namespace Courses.Features.Courses.CreateCourse;

public static class CreateCourseEndpoint
{
    public static void MapCreateCourse(this IEndpointRouteBuilder app)
    {
        app.MapPost("/courses/course", async (CreateCourseCommand command, [FromServices] IInternalEventBus internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return Results.Ok(result);
        }).WithTags("Courses");
    }
}
