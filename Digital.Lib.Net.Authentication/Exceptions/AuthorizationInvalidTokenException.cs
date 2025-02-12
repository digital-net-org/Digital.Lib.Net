using System.Security.Authentication;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class AuthorizationInvalidTokenException() : AuthenticationException("Invalid Token.");