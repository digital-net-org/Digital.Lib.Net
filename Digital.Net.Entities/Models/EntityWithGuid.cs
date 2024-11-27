using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital.Net.Entities.Models;

/// <summary>
///     Base class for entities with a Guid primary key
/// </summary>
public abstract class EntityWithGuid : EntityBase
{
    [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
}
