namespace Blog.Backend.GraphQL;

using HotChocolate;
using Inputs;
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
    public async Task<BlogPostConnection> ListBlogPostsAsync([Service] IStorageService storageService, BlogPostsConnectionInput? input)
    {
        return await storageService.ListBlogPostsAsync(input);
    }
}
