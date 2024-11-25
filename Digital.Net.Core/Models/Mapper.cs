namespace Digital.Net.Core.Models;

public static class Mapper
{

    /// <summary>
    ///     Maps an instance of type T to an instance of type TM.
    /// </summary>
    /// <param name="instance">The instance to map.</param>
    /// <typeparam name="T">The type of the instance to map.</typeparam>
    /// <typeparam name="TM">The type to map to.</typeparam>
    /// <returns>The mapped instance.</returns>
    public static TM Map<T, TM>(T instance) where T : class where TM : class
    {
        var constructor = typeof(TM).GetConstructor([instance.GetType()]);
        return constructor is not null
            ? (TM)constructor.Invoke([instance])
            : throw new InvalidOperationException($"Map error: No suitable constructor found for type {typeof(TM).Name}");
    }

    /// <summary>
    ///     Maps a List of instance of type T to a List of instance of type TM.
    /// </summary>
    /// <param name="instances">The List of instances to map.</param>
    /// <typeparam name="T">The type of the instances to map.</typeparam>
    /// <typeparam name="TM">The type to map to.</typeparam>
    /// <returns>The mapped List of instances.</returns>
    public static List<TM> Map<T, TM>(List<T> instances) where T : class where TM : class =>
        instances.Select(Map<T, TM>).ToList();
}