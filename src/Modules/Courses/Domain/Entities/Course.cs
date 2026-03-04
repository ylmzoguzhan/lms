namespace Courses.Domain.Entities;

public class Course : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; set; }
    public ICollection<Module> Modules { get; set; }
    public Course(string title, string description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
    }
}
