namespace Courses.Features.Lessons.CreateLesson;

public static class CreateLessonEndpoint
{
    public static void MapCreateLesson(this IEndpointRouteBuilder app)
    {
        app.MapPost("/courses/lesson", async (CreateLessonCommand command, [FromServices] IDispatcher internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return result;
        }).WithTags("Courses").RequireAuthorization();
    }
}
