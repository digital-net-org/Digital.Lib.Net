using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Safari.Net.Core.Models;
using Safari.Net.Data.Entities.Models;
using Safari.Net.Data.Repositories;

namespace Safari.Net.Data.Entities;

public abstract class EntityService<T, TQuery>(IRepository<T> repository) : IEntityService<T, TQuery>
    where T : EntityBase
    where TQuery : Query
{
    public QueryResult<TM> Get<TM>(TQuery query) where TM : class
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

    protected abstract Expression<Func<T, bool>> Filter(TQuery query);
}