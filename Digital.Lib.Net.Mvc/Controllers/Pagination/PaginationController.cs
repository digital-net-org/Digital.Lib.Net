using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Digital.Lib.Net.Core.Models;
using Digital.Lib.Net.Core.Predicates;
using Digital.Lib.Net.Entities.Models;
using Digital.Lib.Net.Entities.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital.Lib.Net.Mvc.Controllers.Pagination;

[Route("[controller]")]
public abstract class PaginationController<T, TContext, TDto, TQuery>(
    IRepository<T, TContext> repository
) : ControllerBase
    where T : Entity
    where TContext : DbContext
    where TDto : class
    where TQuery : Query
{
    [HttpGet("")]
    public ActionResult<QueryResult<TDto>> Get([FromQuery] TQuery query)
    {
        query.ValidateParameters();
        var result = new QueryResult<TDto>();
        try
        {
            var items = repository.Get(Filter(query));
            var rowCount = items.Count();
            items = items.AsNoTracking();
            items = items.Skip((query.Index - 1) * query.Size).Take(query.Size);
            items = items.OrderBy(query.OrderBy ?? "CreatedAt");
            result.Value = Mapper.TryMap<T, TDto>(items.ToList());
            result.Total = rowCount;
            result.Index = query.Index;
            result.Size = query.Size;
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return Ok(result);
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

    protected virtual Expression<Func<T, bool>> Filter(Expression<Func<T, bool>> predicate, TQuery query) => predicate;
}