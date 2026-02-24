using Shared.Abstractions.Mediator;

namespace Courses.Contracts;

public record GetCourseExistenceQuery(Guid CourseId) : IQuery<bool>;
