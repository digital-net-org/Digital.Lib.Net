using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using Safari.Net.Core.Messages;
using Safari.Net.Core.Models;
using Safari.Net.Data.Entities.Models;
using Safari.Net.Data.Repositories;

namespace Safari.Net.Data.Entities;

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

    public Result<TM> Get<TM>(Guid? id)
        where TM : class
    {
        var result = new Result<TM>();
        var entity = repository.GetById(id);
        if (entity is null)
            return result.AddError(new InvalidOperationException("Entity not found."));
        result.Value = Mapper.Map<T, TM>(entity);
        return result;
    }

    public Result<TM> Get<TM>(int id)
        where TM : class
    {
        var result = new Result<TM>();
        var entity = repository.GetById(id);
        if (entity is null)
            return result.AddError(new InvalidOperationException("Entity not found."));
        result.Value = Mapper.Map<T, TM>(entity);
        return result;
    }

    public async Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, Guid? id)
        where TM : class
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null
            ? new Result<TM>().AddError(new InvalidOperationException("Entity not found."))
            : await Patch<TM>(patch, entity);
    }

    public async Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, int id)
        where TM : class
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null
            ? new Result<TM>().AddError(new InvalidOperationException("Entity not found."))
            : await Patch<TM>(patch, entity);
    }

    public async Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, T entity)
        where TM : class
    {
        var result = new Result<TM>();
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

    protected abstract Expression<Func<T, bool>> Filter(TQuery query);

    protected abstract void ValidatePatch(Operation<T> patch, T entity);
}
