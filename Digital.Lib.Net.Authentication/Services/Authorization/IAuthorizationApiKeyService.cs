using Digital.Lib.Net.Authentication.Models;
using Digital.Lib.Net.Entities.Models;

namespace Digital.Lib.Net.Authentication.Services.Authorization;

public interface IAuthorizationApiKeyService<TApiUser> : IAuthorizationService
    where TApiUser : EntityGuid, IApiUser;