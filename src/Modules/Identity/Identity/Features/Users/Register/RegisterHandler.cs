using System.Security.Claims;
using System.Text.Json;
using Identity.Contracts;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Request;

namespace Identity.Features.Users.Register;

public record RegisterCommand(string Email, string Password, string Role = "Student", Dictionary<string, string>? Claims = null) : ICommand<Guid>;
public class RegisterHandler(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IdentityDbContext dbContext) : ICommandHandler<RegisterCommand, Guid>
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

            var roleName = string.IsNullOrWhiteSpace(command.Role) ? "Student" : command.Role.Trim();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!roleResult.Succeeded)
                {
                    throw new Exception(string.Join(", ", roleResult.Errors.Select(x => x.Description)));
                }
            }

            var addRoleResult = await userManager.AddToRoleAsync(user, roleName);
            if (!addRoleResult.Succeeded)
            {
                throw new Exception(string.Join(", ", addRoleResult.Errors.Select(x => x.Description)));
            }

            var customClaims = command.Claims?
                .Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value))
                .Select(x => new Claim(x.Key, x.Value))
                .ToList() ?? new List<Claim>();

            if ((roleName.Equals("Instructor", StringComparison.OrdinalIgnoreCase) ||
                roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase)) &&
                customClaims.All(x => x.Type != "permission" || x.Value != "courses:write"))
            {
                customClaims.Add(new Claim("permission", "courses:write"));
            }

            foreach (var claim in customClaims)
            {
                var claimResult = await userManager.AddClaimAsync(user, claim);
                if (!claimResult.Succeeded)
                {
                    throw new Exception(string.Join(", ", claimResult.Errors.Select(x => x.Description)));
                }
            }

            return user.Id;
        }
        throw new Exception("Kayıt başarısız!");
    }
}
