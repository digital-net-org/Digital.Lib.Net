using System.Security.Authentication;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class AuthorizationExpiredTokenException() : AuthenticationException("Token is expired");