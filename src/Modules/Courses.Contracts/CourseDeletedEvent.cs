namespace Courses.Contracts;

public record CourseDeletedEvent(Guid id) : IInternalEvent;

