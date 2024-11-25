using Digital.Net.Core.Interval;

namespace Digital.Net.Entities.Entities;

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