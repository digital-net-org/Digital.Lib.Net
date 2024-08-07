using Safari.Net.Data.Entities.Models;

namespace Safari.Net.Data.Entities;

public interface IEntityService<T, in TQuery> where T : EntityBase where TQuery : Query
{
    /// <summary>
    ///     Get entities based on a query. Converts the entities to the provided model.
    /// </summary>
    /// <param name="query">The query to filter entities</param>
    /// <typeparam name="TM">The model to convert the entities to</typeparam>
    /// <returns>QueryResult of the model</returns>
    QueryResult<TM> Get<TM>(TQuery query) where TM : class;
}