namespace Blog.Backend.GraphQL;

using HotChocolate;
using Models;
using Services;

public class Query
{
    [GraphQLName("blogPost")]
    public async Task<BlogPost> GetBlogPostAsync([Service] IStorageService storageService, string id)
    {
        return await storageService.GetBlogPostAsync(id);
    }
    
    [GraphQLName("blogPosts")]
    public async Task<List<BlogPost>> ListBlogPostsAsync([Service] IStorageService storageService)
    {
        return await storageService.ListBlogPostsAsync();
    }
}
