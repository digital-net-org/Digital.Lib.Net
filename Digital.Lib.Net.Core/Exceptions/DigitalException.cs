using Digital.Lib.Net.Core.Extensions.StringUtilities;

namespace Digital.Lib.Net.Core.Exceptions;

public abstract class DigitalException : Exception
{
    protected DigitalException()
    {
        OnInstantiation();
    }

    protected DigitalException(string message) : base(message)
    {
        OnInstantiation();
    }

    private void OnInstantiation() => Code = GetType().Name.ToUpperSnakeCase();

    public string Code { get; private set; }
}