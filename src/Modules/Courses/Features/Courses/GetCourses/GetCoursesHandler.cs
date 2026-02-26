using Courses.Features.Courses.Dto;
using Shared.Abstractions.Response;

namespace Courses.Features.Courses.GetCourses;

public record GetCoursesQuery(int page, int pageSize) : IQuery<Result<PagedList<CourseDto>>>;
public class GetCoursesHandler(CoursesDbContext dbContext, IPagedListFactory pagedListFactory) : IQueryHandler<GetCoursesQuery, Result<PagedList<CourseDto>>>
{
    public async Task<Result<PagedList<CourseDto>>> HandleAsync(GetCoursesQuery query, CancellationToken ct = default)
    {
        var courseQuery = dbContext.Courses
             .AsNoTracking()
             .Select(x => new CourseDto(x.Id, x.Title));

        var courses = await pagedListFactory.CreateAsync(
            courseQuery,
            query.page,
            query.pageSize,
            ct);

        return new Result<PagedList<CourseDto>>(courses, true, null, 200);
    }
}
