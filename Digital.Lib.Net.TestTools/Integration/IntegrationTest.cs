using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Digital.Lib.Net.TestTools.Integration;

public abstract class IntegrationTest<T, TContext> : UnitTest, IClassFixture<AppFactory<T, TContext>>
    where T : class
    where TContext : DbContext
{
    protected readonly List<HttpClient> Clients = [];
    protected readonly WebApplicationFactory<T> Factory;

    protected HttpClient BaseClient => Clients.First();

    protected IntegrationTest(AppFactory<T, TContext> fixture)
    {
        Factory = fixture;
        Clients.Add(Factory.CreateClient());
    }

    protected TService GetService<TService>() where TService : notnull =>
        Factory.Services.GetRequiredService<TService>();

    protected void CreateClient(int? amount = 1)
    {
        for (var i = 0; i < amount; i++)
        {
            var client = Factory.CreateClient();
            Clients.Add(client);
        }
    }

    protected void DisposeClients()
    {
        foreach (var client in Clients)
            client.Dispose();
        Clients.Clear();
    }
}
