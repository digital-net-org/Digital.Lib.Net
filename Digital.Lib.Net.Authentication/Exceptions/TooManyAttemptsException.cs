using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class TooManyAttemptsException() : DigitalException("Too many login attempts");