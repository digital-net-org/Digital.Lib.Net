using Digital.Lib.Net.Authentication;
using Digital.Lib.Net.Authentication.Options.Jwt;
using Digital.Lib.Net.Database;
using Digital.Lib.Net.Database.Options;
using Digital.Lib.Net.Entities;
using InternalTestProgram.Models;

namespace InternalTestProgram;

public sealed class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddDbConnector<TestContext>(options => options.SetDatabaseEngine(DatabaseEngine.SqLiteInMemory));
        builder.Services.AddControllers();
        builder.Services.AddDigitalEntities<TestContext>();
        builder.Services.AddDigitalJwtOptions(options =>
            options.SetJwtTokenOptions(new JwtTokenOptions
            {
                Issuer = "Issuer",
                Audience = "Audience",
                CookieName = "Cookie",
                Secret = "ThisIsA32ByteSecretKeyForTestingPurposes"
            })
        );
        builder.Services.AddDigitalJwtAuthentication<TestUser, ApiToken, AuthEvent>();
        builder.Services.AddDigitalApiKeyAuthentication<TestUser, ApiKey>();
        builder.Services.AddDigitalApiKeyAuthentication<FakeUser, ApiKey>();


        var app = builder.Build();
        app.MapControllers();

        await app.RunAsync();
    }
}