namespace Users.Features.Enrollments;

public record EnrollInCourseCommand(Guid UserId, Guid CourseId) : ICommand<Guid>;
public class EnrollInCourseHandler(UsersDbContext dbContext, IInternalBus bus) : ICommandHandler<EnrollInCourseCommand, Guid>
{
    public async Task<Guid> HandleAsync(EnrollInCourseCommand command, CancellationToken ct = default)
    {
        var courseExists = await bus.QueryAsync(new GetCourseExistenceQuery(command.CourseId), ct);

        if (!courseExists)
        {
            throw new Exception("Kayıt olunmak istenen kurs bulunamadı!");
        }

        var enrollment = new Enrollment(command.UserId, command.CourseId);
        dbContext.Enrollments.Add(enrollment);
        await dbContext.SaveChangesAsync(ct);

        return enrollment.Id;
    }
}
