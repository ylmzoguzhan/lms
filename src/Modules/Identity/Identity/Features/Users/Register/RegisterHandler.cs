using System.Text.Json;
using Identity.Contracts;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Mediator;

namespace Identity.Features.Users.Register;

public record RegisterCommand(string Email, string Password) : ICommand<Guid>;
public class RegisterHandler(UserManager<AppUser> userManager, IdentityDbContext dbContext) : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Guid> HandleAsync(RegisterCommand command, CancellationToken ct = default)
    {
        var user = new AppUser { UserName = command.Email, Email = command.Email };
        var result = await userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            var outbox = new OutboxMessage
            {
                Type = typeof(UserRegisteredEvent).AssemblyQualifiedName,
                Content = JsonSerializer.Serialize(new UserRegisteredEvent(user.Id, user.Email, user.UserName)),
                CreatedAt = DateTime.UtcNow
            };
            dbContext.OutboxMessages.Add(outbox);
            await dbContext.SaveChangesAsync(ct);

            return user.Id;
        }
        throw new Exception("Kayıt başarısız!");
    }
}
