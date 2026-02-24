using Shared.Abstractions.Mediator;

namespace Courses.Contracts;

public record CourseCreatedEvent(Guid Id, string Title) : IInternalEvent;