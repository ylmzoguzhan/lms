namespace Courses.Domain.Entities;

public class LessonMedia : BaseEntity
{
    public Guid LessonId { get; private set; }
    public Guid MediaId { get; private set; }
    public string FileName { get; private set; }
    public LessonMedia(Guid lessonId, Guid mediaId, string fileName)
    {
        LessonId = lessonId;
        MediaId = mediaId;
        FileName = fileName;
    }
}
