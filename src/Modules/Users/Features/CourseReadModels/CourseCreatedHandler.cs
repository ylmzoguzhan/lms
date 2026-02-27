namespace Users.Features.CourseReadModels;

public class CourseCreatedHandler(UsersDbContext dbContext)
    : IInternalEventHandler<CourseCreatedEvent>
{
    public async Task HandleAsync(CourseCreatedEvent @event, CancellationToken ct)
    {
        var readModel = new CourseReadModel(@event.Id, @event.Title);

        dbContext.CourseReadModels.Add(readModel);
        await dbContext.SaveChangesAsync(ct);
    }
}
