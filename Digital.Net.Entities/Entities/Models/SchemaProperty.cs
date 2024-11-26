using System.Reflection;
using Digital.Net.Entities.Attributes;

namespace Digital.Net.Entities.Entities.Models;

public class SchemaProperty<T>(PropertyInfo propertyInfo) where T : EntityBase
{
    public string Name { get; set; } = propertyInfo.Name;
    public string Path { get; set; } = AttributeAnalyzer<T>.GetPath(propertyInfo);
    public string Type { get; set; } = propertyInfo.PropertyType.Name;
    public string? DataFlag { get; set; } = propertyInfo.GetCustomAttribute<DataFlagAttribute>()?.Flag;
    public bool IsMutable { get; set; } = AttributeAnalyzer<T>.IsMutable(propertyInfo);
    public bool IsSecret { get; set; } = AttributeAnalyzer<T>.IsSecret(propertyInfo);
    public bool IsRequired { get; set; } = AttributeAnalyzer<T>.IsRequired(propertyInfo);
    public bool IsUnique { get; set; } = AttributeAnalyzer<T>.IsUnique(propertyInfo);
    public int? MaxLength { get; set; } = AttributeAnalyzer<T>.MaxLength(propertyInfo);
    public bool IsIdentity { get; set; } = AttributeAnalyzer<T>.IsIdentity(propertyInfo);
    public bool IsForeignKey { get; set; } = AttributeAnalyzer<T>.IsForeignKey(propertyInfo);
}