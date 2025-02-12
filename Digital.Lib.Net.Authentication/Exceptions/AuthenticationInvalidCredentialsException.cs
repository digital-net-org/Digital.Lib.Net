using System.Security.Authentication;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class AuthenticationInvalidCredentialsException() : AuthenticationException("Provided credentials are invalid.");