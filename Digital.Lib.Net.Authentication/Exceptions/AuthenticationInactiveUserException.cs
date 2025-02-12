using System.Security.Authentication;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class AuthenticationInactiveUserException() : AuthenticationException("User is inactive");