using Digital.Lib.Net.Entities.Models.Users;

namespace Digital.Lib.Net.Authentication.Services.Authentication;

public interface IUserContextService
{
    public Guid GetUserId();
    public User GetUser();
}