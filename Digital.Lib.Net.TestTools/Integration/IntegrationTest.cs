using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Digital.Lib.Net.TestTools.Integration;

public abstract class IntegrationTest<T> : UnitTest, IClassFixture<AppFactory<T>>
    where T : class
{
    protected readonly List<HttpClient> Clients = [];
    protected readonly WebApplicationFactory<T> Factory;

    protected HttpClient BaseClient => Clients.First();

    protected IntegrationTest(AppFactory<T> fixture)
    {
        Factory = fixture;
        Clients.Add(Factory.CreateClient());
    }

    protected TService GetService<TService>() where TService : notnull =>
        Factory.Services.GetRequiredService<TService>();

    protected IRepository<TEntity, TContext> GetRepository<TEntity, TContext>()
        where TContext : DbContext
        where TEntity : Entity
    {
        var context = Factory.Services.GetRequiredService<TContext>();
        return new Repository<TEntity, TContext>(context);
    }

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
