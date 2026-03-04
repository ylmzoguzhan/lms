namespace Courses.Domain.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; set; }
    public LessonType LessonType { get; set; }
    public Guid ModuleId { get; set; }
    public Module Module { get; set; }
    private readonly List<LessonMedia> _medias = new();
    public IReadOnlyCollection<LessonMedia> Medias => _medias;
    public Lesson(string title, LessonType lessonType, Guid moduleId)
    {
        Title = title;
        LessonType = lessonType;
        ModuleId = moduleId;
    }
    public void AddMedia(Guid mediaId, string fileName)
    {
        _medias.Add(new LessonMedia(this.Id, mediaId, fileName));
    }
}

public enum LessonType
{
    Video,
    Text
}
