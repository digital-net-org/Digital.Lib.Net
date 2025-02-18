using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class TokenNotFoundException() : DigitalException("Token could not be found in the request headers");
