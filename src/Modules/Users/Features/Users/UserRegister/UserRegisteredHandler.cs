using Identity.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Users.Features.Users.UserRegister;

public class UserRegisteredHandler(UsersDbContext dbContext)
    : IInternalEventHandler<UserRegisteredEvent>
{
    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken ct)
    {
        if (await dbContext.Users.AnyAsync(x => x.Id == @event.UserId, ct))
            return;
        var newUser = new User(@event.UserId, @event.Email);


        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync(ct);
    }
}