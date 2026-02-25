namespace Courses.Features.Courses.CreateCourse;

public static class CreateCourseEndpoint
{
    public static void MapCreateCourse(this IEndpointRouteBuilder app)
    {
        app.MapPost("/courses/course", async (CreateCourseCommand command, [FromServices] IInternalBus internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return Results.Ok(result);
        }).WithTags("Courses").RequireAuthorization();
    }
}
