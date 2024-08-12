namespace Safari.Net.TestTools.Data.Factories;

public static class DataFactoryUtils
{
    public static object ResolveId<T>(T entity)
    {
        var idProperty = typeof(T).GetProperty("Id");
        var idType = idProperty?.PropertyType;
        var id = idProperty?.GetValue(entity);

        if (idProperty is null || id is null || (idType != typeof(int) && idType != typeof(Guid)))
            throw new InvalidOperationException(
                "Entity does not have an int or Guid Id property and thus cannot be created.");

        return id;
    }
}