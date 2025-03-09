using Digital.Lib.Net.Entities.Models.ApiTokens;

namespace Digital.Lib.Net.Authentication.Services.Authentication;

public interface IAuthenticationJwtService
{
    public Task RevokeTokenAsync(string token);
    public Task RevokeAllTokensAsync(Guid userId);
    public string GenerateBearerToken(Guid userId);
    public string GenerateRefreshToken(Guid userId);
}