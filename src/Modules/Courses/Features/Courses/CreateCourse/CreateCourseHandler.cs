using Shared.Abstractions.Auth;
using Shared.Abstractions.Response;

namespace Courses.Features.Courses.CreateCourse;

public record CreateCourseCommand(string title, string description) : ICommand<Result<Guid>>;

public class CreateCourseHandler(CoursesDbContext dbContext) : ICommandHandler<CreateCourseCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(CreateCourseCommand command, CancellationToken ct = default)
    {
        var course = new Course(command.title, command.description);
        dbContext.Courses.Add(course);
        var outbox = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(CourseCreatedEvent).AssemblyQualifiedName,
            Content = JsonSerializer.Serialize(new CourseCreatedEvent(course.Id, course.Title)),
            CreatedAt = DateTime.UtcNow
        };
        dbContext.OutboxMessages.Add(outbox);
        await dbContext.SaveChangesAsync(ct);
        return new Result<Guid>(course.Id, true);
    }
}
