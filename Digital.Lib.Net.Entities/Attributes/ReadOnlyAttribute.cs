namespace Digital.Lib.Net.Entities.Attributes;

/// <summary>
///     Indicates that the property cannot be patched.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ReadOnlyAttribute : Attribute
{
}