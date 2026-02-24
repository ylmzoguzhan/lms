

namespace Users.Features.Enrollments;

public static class EnrollInCourseEndpoint
{
    public static void MapEnrollInCourse(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/enrollincourse", async (EnrollInCourseCommand command, [FromServices] IInternalBus internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return Results.Ok(result);
        }).WithTags("Users");
    }
}
