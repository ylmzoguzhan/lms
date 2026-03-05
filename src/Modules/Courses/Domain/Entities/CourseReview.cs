namespace Courses.Domain.Entities;

public class CourseReview : BaseEntity
{
    public Guid CourseId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }

}
