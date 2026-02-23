namespace Courses.Domain.Entities;

public class Course
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; set; }
    public Course(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
