using System.Security.Claims;

namespace Shared.Abstractions.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, List<string> roles, IEnumerable<Claim>? customClaims = null);
}
