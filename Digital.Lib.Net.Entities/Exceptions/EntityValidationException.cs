using Digital.Lib.Net.Core.Exceptions;

namespace Digital.Lib.Net.Entities.Exceptions;

public class EntityValidationException(string? message) :
    DigitalException(message ?? "Could not validate entity payload");