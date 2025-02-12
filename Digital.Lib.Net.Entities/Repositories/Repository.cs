using System.Collections;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Digital.Lib.Net.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Entities.Repositories;

/// <summary>
///     Repository class for a database context.
/// </summary>
/// <typeparam name="T">The entity type. (Must inherit from EntityBase)</typeparam>
public class Repository<T>(DbContext context) : IRepository<T>
    where T : Entity
{
    public void Create(T entity) => context.Set<T>().Add(entity);

    public async Task CreateAsync(T entity) => await context.Set<T>().AddAsync(entity);

    public void Update(T entity)
    {
        UpdateNestedEntities(entity);
        context.Set<T>().Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        var enumerable = entities as T[] ?? entities.ToArray();
        foreach (var entity in enumerable)
            UpdateNestedEntities(entity);
        context.Set<T>().UpdateRange(enumerable);
    }

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public IQueryable<T> Get(Expression<Func<T, bool>> expression) =>
        context.Set<T>().Where(expression);

    public IQueryable<T> DynamicQuery(string predicate, params object?[] args) =>
        context.Set<T>().Where(predicate, args);

    public T? GetById(int? id) => context.Set<T>().Find(id);

    public T? GetById(Guid? id) => context.Set<T>().Find(id);

    public async Task<T?> GetByIdAsync(int? id) => await context.Set<T>().FindAsync(id);

    public async Task<T?> GetByIdAsync(Guid? id) => await context.Set<T>().FindAsync(id);

    public int Count(Expression<Func<T, bool>> expression) => context.Set<T>().Count(expression);

    public Task<int> CountAsync(Expression<Func<T, bool>> expression) => context.Set<T>().CountAsync(expression);

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

    private void UpdateNestedEntities(T entity)
    {
        var properties = entity.GetType().GetProperties();
        foreach (var property in properties)
        {
            if (!property.PropertyType.IsGenericType ||
                property.PropertyType.GetGenericTypeDefinition() != typeof(List<>))
                continue;
            var values = property.GetValue(entity);

            if (values is null)
                continue;

            foreach (var item in (IEnumerable)values)
                context.Entry(item).State = EntityState.Modified;
        }
    }

    private void AddTimestamps()
    {
        var now = DateTime.UtcNow;
        var entities = context
            .ChangeTracker.Entries()
            .Where(x =>
                x is { Entity: Entity, State: EntityState.Added or EntityState.Modified }
            );
        foreach (var entity in entities)
        {
            var property = entity.State is EntityState.Added ? "CreatedAt" : "UpdatedAt";
            entity.Property(property).CurrentValue = now;
        }
    }
}
