namespace Safari.Net.Core.Models;

public static class Mapper
{
    public static TM Map<T, TM>(T instance) where T : class where TM : class
    {
        var constructor = typeof(TM).GetConstructor([instance.GetType()]);
        return constructor is not null
            ? (TM)constructor.Invoke([instance])
            : throw new InvalidOperationException($"Map error: No suitable constructor found for type {typeof(TM).Name}");
    }
}