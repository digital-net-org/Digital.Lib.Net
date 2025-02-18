using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Files.Exceptions;

public class UnsupportedFormatException() : DigitalException("File format isn't supported");