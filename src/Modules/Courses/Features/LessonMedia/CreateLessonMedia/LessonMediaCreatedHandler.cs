
namespace Courses.Features.LessonMedia.CreateLessonMedia;

public class LessonMediaCreatedHandler(CoursesDbContext dbContext) : IInternalEventHandler<LessonMediaCreatedEvent>
{
    public async Task HandleAsync(LessonMediaCreatedEvent @event, CancellationToken ct = default)
    {
        var lessonMedia = new Domain.Entities.LessonMedia(@event.LessonId, @event.MediaId, @event.FileName);
        await dbContext.LessonMedias.AddAsync(lessonMedia, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}
