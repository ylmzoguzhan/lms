namespace Users.Domain.Entities;

public class Enrollment
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CourseId { get; private set; }
    public DateTime EnrolledAt { get; private set; }

    public Enrollment(Guid userId, Guid courseId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CourseId = courseId;
        EnrolledAt = DateTime.UtcNow;
    }
}