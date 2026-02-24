namespace Users.Domain.Entities;

public class CourseReadModel
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public CourseReadModel(Guid id, string title)
    {
        Id = id;
        Title = title;
    }
}
