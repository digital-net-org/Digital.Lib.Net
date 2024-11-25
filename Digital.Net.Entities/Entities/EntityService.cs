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

public abstract class EntityService<T, TQuery>(IRepository<T> repository)
    : IEntityService<T, TQuery>
    where T : EntityBase
    where TQuery : Query
{
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
            result.Value = Mapper.Map<T, TM>(items.ToList());
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
        result.Value = Mapper.Map<T, TM>(entity);
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
            foreach (var o in patch.Operations)
            {
                if (o.path is "/updated_at" or "/created_at" or "/id")
                    throw new InvalidOperationException($"{o.path}: This field cannot be updated.");

                ValidatePatch(o, entity);
            }

            patch.ApplyTo(entity);
            repository.Update(entity);
            await repository.SaveAsync();
            result.Value = Mapper.Map<T, TM>(entity);
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

        return predicate.Add(Filter(predicate, query));
    }

    protected abstract Expression<Func<T, bool>> Filter(Expression<Func<T, bool>> predicate, TQuery query);

    protected abstract void ValidatePatch(Operation<T> patch, T entity);
}
