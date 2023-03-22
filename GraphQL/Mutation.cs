namespace Blog.Backend.GraphQL;

using HotChocolate;
using Models;
using Services;

public class Mutation
{
    [GraphQLName("uploadAsset")]
    public async Task<Asset> UploadAssetAsync([Service] IStorageService storageService, IFile file)
    {
        return await storageService.UploadAssetAsync(file);
    }
    
    [GraphQLName("publish")]
    public async Task<bool> Publish([Service] IStorageService storageService, Type type, string id)
    {
        return await storageService.PublishAsync(type, id);
    }
    
    [GraphQLName("unpublish")]
    public async Task<bool> Unpublish([Service] IStorageService storageService, Type type, string id)
    {
        return await storageService.UnpublishAsync(type, id);
    }
}
