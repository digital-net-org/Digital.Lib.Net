using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class ExpiredTokenException() : DigitalException("Token is expired");