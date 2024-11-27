using Digital.Net.Core.Interval;
using Digital.Net.Entities.Services;

namespace Digital.Net.Entities.Models;

public class Query
{
    public int Index { get; set; } = QueryUtils.DefaultIndex;
    public int Size { get; set; } = QueryUtils.DefaultSize;
    public string? OrderBy { get; set; }

    public DateRange? CreatedIn { get; set; }
    public DateRange? UpdatedIn { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}