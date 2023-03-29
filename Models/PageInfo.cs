// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace Blog.Backend.Models;

public class PageInfo
{
    public bool HasNextPage { get; set; }
    public bool HesPreviousPage { get; set; }
    public string? StartCursor { get; set; }
    public string? EndCursor { get; set; }
    public int PageSize { get; set; }
}
