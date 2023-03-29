// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable CS8618
namespace Blog.Backend.Models;

public class BlogPostConnection
{
    public List<Edge> Edges { get; set; }
    public PageInfo PageInfo { get; set; }
}
