namespace Shared.Abstractions.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, List<string> roles, Dictionary<string, string>? customClaims = null);
}