// ReSharper disable ClassNeverInstantiated.Global
namespace Blog.Backend.Models;

public class BlogPost
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Stage { get; set; }
    public Asset? Image { get; set; }
}