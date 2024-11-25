namespace Digital.Net.TestTools;

public abstract class UnitTest
{
    public const string AspnetcoreEnvironment = "Test";

    static UnitTest()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", AspnetcoreEnvironment);
    }
}
