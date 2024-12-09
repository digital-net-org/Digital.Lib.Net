using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital.Net.Entities.Models;

/// <summary>
///     Base class for entities with an integer primary key
/// </summary>
public abstract class EntityWithId : EntityBase
{
    [Column("Id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
}