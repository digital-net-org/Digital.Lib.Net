using Digital.Net.Core.Messages;
using Digital.Net.Entities.Entities.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Digital.Net.Entities.Entities;

public interface IEntityService<T, in TQuery>
    where T : EntityBase
    where TQuery : Query
{
    /// <summary>
    ///     Get entities based on a query. Converts the entities to the provided model.
    /// </summary>
    /// <param name="query">The query to filter entities</param>
    /// <typeparam name="TM">The model to convert the entities to</typeparam>
    /// <returns>QueryResult of the model</returns>
    QueryResult<TM> Get<TM>(TQuery query)
        where TM : class;

    /// <summary>
    ///    Get an entity based on its primary key. Converts the entity to the provided model.
    /// </summary>
    /// <param name="id">The entity primary key</param>
    /// <typeparam name="TM">The model to convert the entities to</typeparam>
    /// <returns>Result of the model</returns>
    Result<TM> Get<TM>(Guid? id)
        where TM : class;

    /// <summary>
    ///    Get an entity based on its primary key. Converts the entity to the provided model.
    /// </summary>
    /// <param name="id">The entity primary key</param>
    /// <typeparam name="TM">The model to convert the entities to</typeparam>
    /// <returns>Result of the model</returns>
    Result<TM> Get<TM>(int id)
        where TM : class;

    /// <summary>
    ///     Patch an entity based on its primary key.
    /// </summary>
    /// <param name="patch">The patch body</param>
    /// <param name="id">The entity primary key</param>
    /// <typeparam name="TM">The model to convert the entities to</typeparam>
    /// <returns>Result of the model</returns>
    /// <exception cref="InvalidOperationException">If the patch is invalid, use the ValidatePatch method to handle exceptions.</exception>
    Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, Guid? id)
        where TM : class;

    /// <summary>
    ///     Patch an entity based on its primary key.
    /// </summary>
    /// <param name="patch">The patch body</param>
    /// <param name="id">The entity primary key</param>
    /// <typeparam name="TM">The model to convert the entities to</typeparam>
    /// <returns>Result of the model</returns>
    /// <exception cref="InvalidOperationException">If the patch is invalid, use the ValidatePatch method to handle exceptions.</exception>
    Task<Result<TM>> Patch<TM>(JsonPatchDocument<T> patch, int id)
        where TM : class;
}
