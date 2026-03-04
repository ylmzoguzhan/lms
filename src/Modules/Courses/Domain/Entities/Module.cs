namespace Courses.Domain.Entities;

public class Module : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }

    public Guid CourseId { get; private set; }
    public Course Course { get; private set; }
    public ICollection<Lesson> Lessons { get; private set; }
    public Module(string title, string description, Guid courseId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        CourseId = courseId;
    }
}
