using System.Text.Json;
using Courses.Contracts;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Shared.Abstractions.Messaging.Internal;

namespace Courses.Features.Courses.CreateCourse;

public record CreateCourseCommand(string title, string description) : ICommand<Guid>;

public class CreateCourseHandler(CoursesDbContext dbContext) : ICommandHandler<CreateCourseCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateCourseCommand command, CancellationToken ct = default)
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
        return course.Id;
    }
}
