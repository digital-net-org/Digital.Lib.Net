using Digital.Lib.Net.Core.Messages;
using Digital.Lib.Net.Entities.Models.Users;

namespace Digital.Lib.Net.Authentication.Services.Authentication;

public interface IAuthenticationService
{
    public Guid? GetAuthenticatedUserId();
    public User? GetAuthenticatedUser();

    public Task<User?> GetAuthenticatedUserAsync();
    Task<int> GetLoginAttemptCountAsync(User? user = null);
    public Result<string> RefreshTokens();
    public Task<Result<User>> ValidateCredentialsAsync(string login, string password);
    public Task<Result<User>> UpdatePasswordAsync(User user, string currentPassword, string newPassword);
    public Task<Result<string>> LoginAsync(string login, string password);
    public Task<Result> LogoutAsync();
    public Task<Result> LogoutAllAsync();
}