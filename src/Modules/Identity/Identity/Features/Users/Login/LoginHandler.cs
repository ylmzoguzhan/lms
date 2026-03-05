using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Auth;
using Shared.Abstractions.Request;

namespace Identity.Features.Users.Login;

public record LoginCommand(string Email, string Password) : ICommand<string>;
public class LoginHandler(UserManager<AppUser> userManager, IJwtTokenGenerator tokenGenerator) : ICommandHandler<LoginCommand, string>
{
    public async Task<string> HandleAsync(LoginCommand command, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, command.Password))
        {
            return "";
        }
        var roles = (await userManager.GetRolesAsync(user)).ToList();
        var claims = await userManager.GetClaimsAsync(user);
        var loginResponse = tokenGenerator.GenerateToken(user.Id, user.Email!, roles, claims);
        return loginResponse;
    }
}
