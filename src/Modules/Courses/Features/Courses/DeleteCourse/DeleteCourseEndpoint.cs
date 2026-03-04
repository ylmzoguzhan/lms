namespace Courses.Features.Courses.DeleteCourse;

public static class DeleteCourseEndpoint
{
    public static void MapDeleteCourse(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/courses/course/{id}", async (Guid id, [FromServices] IDispatcher internalBus) =>
        {
            var result = await internalBus.SendAsync(new DeleteCourseCommand(id));
            return result;
        }).WithTags("Courses").RequireAuthorization();
    }
}
