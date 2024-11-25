using System.Linq.Expressions;
using Digital.Net.Entities.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital.Net.Entities.Repositories;

/// <summary>
///     Repository class for a database context.
/// </summary>
/// <typeparam name="T">The entity type. (Must inherit from EntityBase)</typeparam>
/// <typeparam name="TContext">The database context type.</typeparam>
/// <example>
///     Extends Repository class with your context.
///     <code>
///     public class MyContextRepository&lt;T&gt;(MyContext context) : Repository&lt;T, MyContext&gt;(context)
///         where T : EntityBase;
/// </code>
///     Then implement IRepository using dependency injection.
///     <code>
///     services.AddScoped(typeof(IRepository&lt;&gt;), typeof(MyContextRepository&lt;&gt;));
/// </code>
/// </example>
public class Repository<T, TContext>(TContext context) : IRepository<T>
    where T : EntityBase
    where TContext : DbContext
{
    public void Create(T entity) => context.Set<T>().Add(entity);

    public async Task CreateAsync(T entity) => await context.Set<T>().AddAsync(entity);

    public void Update(T entity) => context.Set<T>().Update(entity);

    public void UpdateRange(IEnumerable<T> entities) => context.Set<T>().UpdateRange(entities);

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public IQueryable<T> Get(Expression<Func<T, bool>> expression) =>
        context.Set<T>().Where(expression);

    public T? GetById(int? id) => context.Set<T>().Find(id);

    public T? GetById(Guid? id) => context.Set<T>().Find(id);

    public async Task<T?> GetByIdAsync(int? id) => await context.Set<T>().FindAsync(id);

    public async Task<T?> GetByIdAsync(Guid? id) => await context.Set<T>().FindAsync(id);

    public async Task SaveAsync()
    {
        AddTimestamps();
        await context.SaveChangesAsync();
    }

    public void Save()
    {
        AddTimestamps();
        context.SaveChanges();
    }

    private void AddTimestamps()
    {
        var now = DateTime.UtcNow;
        var entities = context
            .ChangeTracker.Entries()
            .Where(x =>
                x is { Entity: EntityBase, State: EntityState.Added or EntityState.Modified }
            );
        foreach (var entity in entities)
        {
            var property = entity.State is EntityState.Added ? "CreatedAt" : "UpdatedAt";
            entity.Property(property).CurrentValue = now;
        }
    }
}
