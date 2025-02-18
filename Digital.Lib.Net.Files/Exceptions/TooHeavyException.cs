using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Files.Exceptions;

public class TooHeavyException() : DigitalException("File size is too large.");