using Shared.Abstractions.Messaging.Internal;

namespace Courses.Contracts;

public record GetCourseExistenceQuery(Guid CourseId) : IQuery<bool>;
