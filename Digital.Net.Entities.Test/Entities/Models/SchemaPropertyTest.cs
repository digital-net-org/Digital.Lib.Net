using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digital.Net.Entities.Attributes;
using Digital.Net.Entities.Entities.Models;
using Digital.Net.TestTools;

namespace Digital.Net.Entities.Test.Entities.Models;

public class SchemaPropertyTest : UnitTest
{
    private class TestEntity : EntityBase
    {
        [DataFlag("test_flag")]
        [Required]
        [Column("required_property")]
        public string RequiredProperty { get; set; }
    }

    [Fact]
    public void SchemaProperty_SetsPropertiesCorrectly()
    {
        var propertyInfo = typeof(TestEntity).GetProperty("RequiredProperty");
        var schemaProperty = new SchemaProperty<TestEntity>(propertyInfo!);
        Assert.Equal("RequiredProperty", schemaProperty.Name);
        Assert.Equal("required_property", schemaProperty.Path);
        Assert.Equal(propertyInfo!.PropertyType.Name, schemaProperty.Type);
        Assert.Equal("test_flag", schemaProperty.DataFlag);
        Assert.Null(schemaProperty.RegexValidation);
        Assert.True(schemaProperty.IsRequired);
        Assert.False(schemaProperty.IsReadOnly);
        Assert.False(schemaProperty.IsSecret);
        Assert.False(schemaProperty.IsUnique);
        Assert.False(schemaProperty.IsIdentity);
        Assert.False(schemaProperty.IsForeignKey);
        Assert.Null(schemaProperty.MaxLength);
    }
}