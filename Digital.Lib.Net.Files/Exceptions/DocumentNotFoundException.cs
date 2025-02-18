using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Files.Exceptions;

public class DocumentNotFoundException() : DigitalException("This document could not be found in database");