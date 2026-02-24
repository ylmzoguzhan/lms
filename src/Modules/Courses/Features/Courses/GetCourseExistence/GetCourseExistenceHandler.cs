using Courses.Contracts;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.Internal;

namespace Courses.Features.Courses.GetCourseExistence;



public class GetCourseExistenceHandler(CoursesDbContext dbContext)
    : IQueryHandler<GetCourseExistenceQuery, bool>
{
    public async Task<bool> HandleAsync(GetCourseExistenceQuery query, CancellationToken ct)
    {
        return await dbContext.Courses.AnyAsync(x => x.Id == query.CourseId, ct);
    }
}
