using System.Security.Authentication;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class AuthenticationTooManyAttemptsException() : AuthenticationException("Too many login attempts");