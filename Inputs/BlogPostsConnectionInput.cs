// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Blog.Backend.Inputs;

public class BlogPostsConnectionInput
{
    public string? After { get; set; }
    public string? Before { get; set; }
    public int? First { get; set; }
    public int? Last { get; set; }
    public BlogPostOrderByInput? OrderBy { get; set; }
    public StageInput? Stage { get; set; }
    public List<string>? Tags { get; set; }
}

public enum BlogPostOrderByInput
{
    PublishedAtAsc,
    PublishedAtDesc,
    TitleAsc,
    TitleDesc
}
