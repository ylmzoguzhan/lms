namespace Courses.Features.LessonMedia.CreateLessonMedia;

public record LessonMediaCreatedEvent(Guid LessonId, Guid MediaId, string FileName) : IInternalEvent;
