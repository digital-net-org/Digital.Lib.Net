using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Digital.Lib.Net.TestTools.Integration;

public abstract class IntegrationTest<T> : UnitTest, IClassFixture<AppFactory<T>>, IDisposable
    where T : class
{
    private readonly List<HttpClient> _clients = [];
    private readonly WebApplicationFactory<T> _factory;

    protected HttpClient BaseClient => _clients.First();
    protected List<HttpClient> ClientPool => _clients.Skip(1).ToList();

    protected IntegrationTest(AppFactory<T> fixture)
    {
        _factory = fixture;
        _clients.Add(_factory.CreateClient());
    }

    protected TService GetService<TService>()
        where TService : notnull => _factory.Services.GetRequiredService<TService>();

    protected IRepository<TEntity, TContext> GetRepository<TEntity, TContext>()
        where TContext : DbContext
        where TEntity : Entity
    {
        var context = _factory.Services.GetRequiredService<TContext>();
        return new Repository<TEntity, TContext>(context);
    }

    protected HttpClient CreateClient()
    {
        var result = _factory.CreateClient();
        _clients.Add(result);
        return result;
    }

    protected void CreateClient(int amount)
    {
        for (var i = 0; i < amount; i++)
            _clients.Add(_factory.CreateClient());
    }

    public void Dispose()
    {
        foreach (var client in _clients)
            client.Dispose();

        _clients.Clear();
    }
}
