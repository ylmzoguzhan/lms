using Shared.Abstractions.Exceptions;
using Shared.Abstractions.Response;

namespace Courses.Features.Courses.DeleteCourse;

public record DeleteCourseCommand(Guid id) : ICommand<Result<bool>>;
public class DeleteCourseHandler(CoursesDbContext dbContext) : ICommandHandler<DeleteCourseCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(DeleteCourseCommand command, CancellationToken ct = default)
    {
        var course = await dbContext.Courses.FindAsync(command.id);
        if (course == null)
            throw new NotFoundException($"{command.id} id'li kurs bulunamadı");
        dbContext.Courses.Remove(course);
        var outbox = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(CourseDeletedEvent).AssemblyQualifiedName,
            Content = JsonSerializer.Serialize(new CourseDeletedEvent(course.Id)),
            CreatedAt = DateTime.UtcNow
        };
        dbContext.OutboxMessages.Add(outbox);

        await dbContext.SaveChangesAsync(ct);
        return new Result<bool>(true, true, null, 200);
    }
}
