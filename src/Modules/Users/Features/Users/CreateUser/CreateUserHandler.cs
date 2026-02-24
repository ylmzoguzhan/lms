using Shared.Abstractions.Messaging.Internal;
using Users.Domain.Entities;
using Users.Infrastructure.Data;

namespace Users.Features.Users.CreateUser;

public record CreateUserCommand(string email) : ICommand<Guid>;
public class CreateUserHandler(UsersDbContext dbContext) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        var user = new User(command.email);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(ct);
        return user.Id;
    }
}
