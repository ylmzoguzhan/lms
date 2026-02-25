namespace Courses.Domain.Entities;

public class Course
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; set; }
    public Guid CreatedBy { get; set; }
    public Course(string title, string description, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        CreatedBy = createdBy;
    }
}
