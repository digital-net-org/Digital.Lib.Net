using Safari.Net.Data.Entities.Models;
using Safari.Net.Data.Repositories;

namespace Safari.Net.TestTools.Data.Factories;

public class DataFactory<T>(IRepository<T> repository) where T : EntityBase
{
    /// <summary>
    ///     Creates an entity and returns it.
    /// </summary>
    /// <param name="entity">Entity to create.</param>
    /// <returns>Created entity.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T Create(T? entity = null)
    {
        entity ??= Activator.CreateInstance<T>();
        repository.Create(entity);
        repository.Save();
        var id = DataFactoryUtils.ResolveId(entity);
        entity = repository.GetById(id);
        return entity ?? throw new InvalidOperationException("Entity could not be created.");
    }

    /// <summary>
    ///     Creates an entity and returns it.
    /// </summary>
    /// <param name="entity">Entity to create.</param>
    /// <returns>Created entity.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<T> CreateAsync(T? entity = null)
    {
        entity ??= Activator.CreateInstance<T>();
        await repository.CreateAsync(entity);
        await repository.SaveAsync();
        var id = DataFactoryUtils.ResolveId(entity);
        entity = await repository.GetByIdAsync(id);
        return entity ?? throw new InvalidOperationException("Entity could not be created.");
    }
}