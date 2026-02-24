using Shared.Abstractions.Messaging.Internal;

namespace Courses.Contracts;

public record CourseCreatedEvent(Guid Id, string Title) : IInternalEvent;