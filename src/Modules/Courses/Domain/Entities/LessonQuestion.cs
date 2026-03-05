namespace Courses.Domain.Entities;

public class LessonQuestion : BaseEntity
{
    public Guid LessonId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; private set; }
    public string? Title { get; private set; }
    public string Question { get; private set; }
}
