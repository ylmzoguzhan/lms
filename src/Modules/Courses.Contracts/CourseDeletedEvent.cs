using Shared.Abstractions.Mediator;

namespace Courses.Contracts;

public record CourseDeletedEvent(Guid id) : IInternalEvent;

