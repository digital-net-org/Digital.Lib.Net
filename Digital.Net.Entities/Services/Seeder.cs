using System.Reflection;
using Digital.Net.Core.Messages;
using Digital.Net.Entities.Models;
using Digital.Net.Entities.Repositories;
using Microsoft.Extensions.Logging;

namespace Digital.Net.Entities.Services;

public class Seeder<T>(
    ILogger<Seeder<T>> logger,
    IRepository<T> repository
) : ISeeder<T>
    where T : Entity
{
    public async Task<Result<List<T>>> SeedAsync(List<T> data)
    {
        var result = new Result<List<T>>([]);
        var skip = 0;

        foreach (var entity in data)
            try
            {
                var properties = typeof(T)
                    .GetProperties()
                    .Where(PropertyExclusionPredicate())
                    .ToList();

                if (repository.DynamicQuery(BuildQuery(properties, entity)).Any())
                {
                    skip++;
                    continue;
                }

                await repository.CreateAsync(entity);
                await repository.SaveAsync();
                result.Value!.Add(entity);
            }
            catch (Exception e)
            {
                result.AddError(e);
            }

        if (skip > 0)
            logger.LogInformation($"Skipped {skip} {nameof(T)} entities because they already exist.");
        if (result.HasError)
            logger.LogError($"One or more entities could not be seeded: {result.Errors}");

        return result;
    }

    private static string BuildQuery(List<PropertyInfo> properties, T entity)
    {
        var query = string.Empty;
        foreach (var property in properties)
        {
            var value = property.GetValue(entity);
            if (value is null)
                continue;

            value = property.PropertyType == typeof(string)
                ? $"\"{value}\""
                : value;

            query += $"{(query == string.Empty ? query : " &&")} {property.Name} == {value}";
        }

        return query;
    }

    private static Func<PropertyInfo, bool> PropertyExclusionPredicate() =>
        property =>
            property.Name is not ("Id" or "CreatedAt" or "UpdatedAt")
            && property.PropertyType != typeof(bool);
}