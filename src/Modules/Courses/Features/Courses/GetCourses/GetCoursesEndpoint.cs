namespace Courses.Features.Courses.GetCourses;

public static class GetCoursesEndpoint
{
    public static void MapGetCourses(this IEndpointRouteBuilder app)
    {
        app.MapPost("/courses/courses", async (GetCoursesQuery query, [FromServices] IInternalBus internalBus) =>
        {
            var result = await internalBus.QueryAsync(query);
            return result;
        }).WithTags("Courses");
    }
}
