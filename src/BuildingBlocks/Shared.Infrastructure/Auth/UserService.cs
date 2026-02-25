using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Abstractions.Auth;

namespace Shared.Infrastructure.Auth;

public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    public Guid? UserId =>
    Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
    ? id : null;
}
