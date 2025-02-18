using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Authentication.Exceptions;

public class PasswordMalformedException() : DigitalException("Provided password does not meet requirements.");