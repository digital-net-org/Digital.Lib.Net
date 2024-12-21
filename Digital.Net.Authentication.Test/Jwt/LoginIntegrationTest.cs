using System.Net;
using Digital.Net.Authentication.Models.Events;
using Digital.Net.Authentication.Options;
using Digital.Net.Authentication.Services.Authentication.Events;
using Digital.Net.Core.Extensions.StringUtilities;
using Digital.Net.Entities.Repositories;
using Digital.Net.Http.HttpClient.Extensions;
using Digital.Net.TestTools.Integration;
using InternalTestProgram;
using InternalTestProgram.Controllers;
using InternalTestProgram.Extensions;
using InternalTestProgram.Factories;
using InternalTestProgram.Models;

namespace Digital.Net.Authentication.Test.Jwt;

public class LoginIntegrationTest : IntegrationTest<Program, TestContext>
{
    private readonly TestUserFactory _testUserFactory;
    private readonly Repository<AuthEvent> _authEventRepository;
    private readonly Repository<ApiToken> _apiTokenRepository;

    public LoginIntegrationTest(AppFactory<Program, TestContext> fixture) : base(fixture)
    {
        var context = GetContext();
        _testUserFactory = new TestUserFactory(new Repository<TestUser>(context));
        _authEventRepository = new Repository<AuthEvent>(context);
        _apiTokenRepository = new Repository<ApiToken>(context);
    }

    [Fact]
    public async Task Login_OnSuccess()
    {
        var (user, password) = _testUserFactory.Create();
        var response = await BaseClient.Login(user.Login, password);
        var registeredToken = _apiTokenRepository.Get(x => x.ApiUserId == user.Id).First();
        var record = _authEventRepository.Get(x => x.ApiUserId == user.Id).First();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(registeredToken);

        Assert.True(record is { EventType: AuthenticationEventType.Login, State: ApiEventState.Success });
        Assert.True((await response.Content.ReadAsStringAsync()).IsJsonWebToken());
        Assert.True(response.Headers.TryGetCookie("Cookie")?.IsJsonWebToken());
    }

    [Fact]
    public async Task Login_OnWrongPassword()
    {
        var (user, _) = _testUserFactory.Create();
        var response = await BaseClient.Login(user.Login, "wrongPassword");
        var record = _authEventRepository.Get(x => x.ApiUserId == user.Id).First();

        Assert.True(record is { EventType: AuthenticationEventType.Login, State: ApiEventState.Failed });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_OnInactiveUser()
    {
        var (user, password) = _testUserFactory.Create(new NullableTestUser { IsActive = false });
        var response = await BaseClient.Login(user.Login, password);
        var record = _authEventRepository.Get(x => x.ApiUserId == user.Id).First();
        Assert.True(record is { EventType: AuthenticationEventType.Login, State: ApiEventState.Failed });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_OnMaxCurrentSessions()
    {
        var (user, password) = _testUserFactory.Create();

        CreateClient(AuthenticationDefaults.MaxConcurrentSessions);
        foreach (var client in Clients)
        {
            await client.Login(user.Login, password);
            var response = await client.GetAsync(TestAuthorizeController.TestApiKeyOrJwtRoute);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        var recordsCount = await _authEventRepository.CountAsync(e => e.ApiUserId == user.Id);
        Assert.Equal(AuthenticationDefaults.MaxConcurrentSessions + 1, recordsCount);

        var unauthorizedResponse = await Clients.First().RefreshTokens();
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorizedResponse.StatusCode);
    }
}