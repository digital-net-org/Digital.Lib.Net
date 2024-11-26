using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Digital.Net.Core.Messages;
using Digital.Net.Core.Models;
using Digital.Net.Core.Predicates;
using Digital.Net.Entities.Entities.Models;
using Digital.Net.Entities.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;

namespace Digital.Net.Entities.Entities;

// TODO: Can it be non-abstract?
// TODO: Add Regex validation attribute (should be returned in the schema)
// TODO: Add Unit Tests on patcher exceptions
public abstract class EntityService<T, TQuery>(IRepository<T> repository)
    : IEntityService<T, TQuery>
    where T : EntityBase
    where TQuery : Query
{
    public List<SchemaProperty<T>> GetSchema() =>
        typeof(T).GetProperties().Select(property => new SchemaProperty<T>(property)).ToList();

    public QueryResult<TM> Get<TM>(TQuery query)
        where TM : class
    {
        query.ValidateParameters();
        var result = new QueryResult<TM>();
        try
        {
            var items = repository.Get(Filter(query));
            var rowCount = items.Count();
            items = items.AsNoTracking();
            items = items.Skip((query.Index - 1) * query.Size).Take(query.Size);
            items = items.OrderBy(query.OrderBy ?? "CreatedAt");
            result.Value = Mapper.MapFromConstructor<T, TM>(items.ToList());
            result.Total = rowCount;
            result.Index = query.Index;
            result.Size = query.Size;
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }

    public Result<TM> Get<TM>(Guid? id) where TM : class => Get<TM>(repository.GetById(id));

    public Result<TM> Get<TM>(int id) where TM : class => Get<TM>(repository.GetById(id));

    private static Result<TM> Get<TM>(T? entity) where TM : class
    {
        var result = new Result<TM>();
        if (entity is null)
            return result.AddError(new InvalidOperationException("Entity not found."));
        result.Value = Mapper.MapFromConstructor<T, TM>(entity);
        return result;
    }

    public async Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, Guid? id) where TM : class =>
        await Patch<TM>(patch, await repository.GetByIdAsync(id));

    public async Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, int id) where TM : class =>
        await Patch<TM>(patch, await repository.GetByIdAsync(id));

    private async Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, T? entity)
        where TM : class
    {
        var result = new Result<TM>();
        if (entity is null)
            return result.AddError(new InvalidOperationException("Entity not found."));
        try
        {
            var schema = GetSchema();
            foreach (var o in patch.Operations)
            {
                var property = schema.First(x => x.Path == o.path[1..]);

                if (property.IsIdentity || property.IsForeignKey || !property.IsMutable)
                    throw new InvalidOperationException($"{o.path}: This field is read-only.");

                if (property.IsRequired && o.value is null)
                    throw new InvalidOperationException($"{o.path}: This field is required and cannot be null.");

                if (property is { IsRequired: true, Type: "String" } && string.IsNullOrWhiteSpace(o.value?.ToString()))
                    throw new InvalidOperationException($"{o.path}: This field is required and cannot be empty.");

                if (property is { MaxLength: > 0, Type: "String" } && o.value?.ToString()?.Length > property.MaxLength)
                    throw new InvalidOperationException($"{o.path}: Maximum length exceeded.");

                if (property.IsUnique &&
                    repository.Get(x => EF.Property<object>(x, property.Name).Equals(o.value)).Any())
                    throw new InvalidOperationException($"{o.path}: This value violates a unique constraint.");

                // Regex validations

                ValidatePatch(o, entity); // TODO: Remove this method
            }

            patch.ApplyTo(entity);
            repository.Update(entity);
            await repository.SaveAsync();
            result.Value = Mapper.MapFromConstructor<T, TM>(entity);
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }

    private Expression<Func<T, bool>> Filter(TQuery query)
    {
        var predicate = PredicateBuilder.New<T>();
        if (query.CreatedAt.HasValue)
            predicate = predicate.Add(x => x.CreatedAt >= query.CreatedAt);
        if (query.UpdatedAt.HasValue)
            predicate = predicate.Add(x => x.UpdatedAt >= query.UpdatedAt);
        if (query.CreatedIn is not null)
            predicate = predicate.Add(x => x.CreatedAt >= query.CreatedIn.From && x.CreatedAt <= query.CreatedIn.To);
        if (query.UpdatedIn is not null)
            predicate = predicate.Add(x => x.UpdatedAt >= query.UpdatedIn.From && x.UpdatedAt <= query.UpdatedIn.To);

        return Filter(predicate, query);
    }

    protected abstract Expression<Func<T, bool>> Filter(Expression<Func<T, bool>> predicate, TQuery query);

    protected abstract void ValidatePatch(Operation<T> patch, T entity);
}
