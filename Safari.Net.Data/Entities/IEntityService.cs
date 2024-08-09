using Microsoft.AspNetCore.JsonPatch;
using Safari.Net.Core.Messages;
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

    /// <summary>
    ///     Patch an entity based on its primary key.
    /// </summary>
    /// <param name="patch">The patch body</param>
    /// <param name="id">The entity primary key</param>
    /// <typeparam name="TM">The model to convert the entities to</typeparam>
    /// <returns>Result of the model</returns>
    Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, params object?[]? id) where TM : class;
}