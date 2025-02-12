using Digital.Lib.Net.Core.Environment;

namespace Digital.Lib.Net.TestTools;

public abstract class UnitTest
{
    protected UnitTest()
    {
        AspNetEnv.Set(AspNetEnv.Test);
    }
}
