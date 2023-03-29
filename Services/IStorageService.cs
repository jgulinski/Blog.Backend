// ReSharper disable IdentifierTypo
namespace Blog.Backend.Services;

using Inputs;
using Models;

public interface IStorageService
{
    Task<BlogPost> GetBlogPostAsync(string id);
    Task<BlogPostConnection> ListBlogPostsAsync(BlogPostsConnectionInput? input);
    Task<Asset> UploadAssetAsync(IFile file);
    Task<bool> PublishAsync(Type type, string id);
    Task<bool> UnpublishAsync(Type type, string id);
}
