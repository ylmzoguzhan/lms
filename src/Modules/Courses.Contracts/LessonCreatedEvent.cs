namespace Courses.Contracts;

public record LessonCreatedEvent(string FileName) : IInternalEvent;
