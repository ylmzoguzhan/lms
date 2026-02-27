
using Shared.Abstractions.Exceptions;

namespace Users.Features.CourseReadModels;

public class CourseDeletedHandler(UsersDbContext dbContext) : IInternalEventHandler<CourseDeletedEvent>
{
    public async Task HandleAsync(CourseDeletedEvent @event, CancellationToken ct = default)
    {
        var readModel = await dbContext.CourseReadModels.FindAsync(@event.id);
        if (readModel == null)
            throw new NotFoundException($"{@event.id} id'li kurs bulunamadı");
        dbContext.CourseReadModels.Remove(readModel);
        await dbContext.SaveChangesAsync(ct);
    }
}
