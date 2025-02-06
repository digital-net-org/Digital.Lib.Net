namespace Digital.Net.Entities.Models;

public static class EntityTypeExtensions
{
    public static bool IsEntity(this Type type) =>
        type.BaseType == typeof(EntityId) || type.BaseType == typeof(EntityGuid);
}