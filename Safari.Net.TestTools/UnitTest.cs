namespace Safari.Net.TestTools;

public abstract class UnitTest
{
    public const string ASPNETCORE_ENVIRONMENT = "Test";

    static UnitTest()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", ASPNETCORE_ENVIRONMENT);
    }
}
