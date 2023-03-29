// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
namespace Blog.Backend.Services;

using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using global::GraphQL;
using global::GraphQL.Client.Http;
using global::GraphQL.Client.Serializer.SystemTextJson;
using Inputs;
using Models;
using Newtonsoft.Json.Linq;
using Utils;

public class HygraphService : IStorageService
{
    private readonly Settings _settings;

    public HygraphService(Settings settings)
    {
        _settings = settings;
    }

    public async Task<BlogPost> GetBlogPostAsync(string id)
    {
        var client = new GraphQLHttpClient(new Uri(_settings.HygraphContentApiUrl), new SystemTextJsonSerializer());
        var request = new GraphQLRequest
        {
            Query = $@"
                    query {{
                      blogPost(where: {{id: ""{id}""}}) {{
                        id
                        title
                        content
                        stage
                        tags
                        image {{
                          id
                          url
                          mimeType
                          size
                          width
                          height
                          fileName
                        }}
                      }}
                    }}"
        };

        client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.HygraphSecret);

        var response = await client.SendQueryAsync<JsonElement>(request);
        var blogPost = JsonUtil.Deserialize<BlogPost>(response.Data.ToString(), ignoreRoot: true);

        if (blogPost == null)
        {
            throw new Exception("Blog post not found");
        }

        if (response.Errors != null && response.Errors?.Length != 0)
        {
            throw new Exception(response.Errors?[0].Message);
        }

        return blogPost;
    }
    
    public async Task<BlogPostConnection> ListBlogPostsAsync(BlogPostsConnectionInput? input)
    {
        var client = new GraphQLHttpClient(new Uri(_settings.HygraphContentApiUrl), new SystemTextJsonSerializer());

        string? arguments = null;
        if (input != null)
        {
            arguments = "(";
            if (input.After != null) arguments = string.Concat(arguments, $"after: \"{input.After}\"");
            if (input.Before != null) arguments = string.Concat(arguments, $" before: \"{input.Before}\"");
            if (input.First != null) arguments = string.Concat(arguments, $" first: {input.First}");
            if (input.Last != null) arguments = string.Concat(arguments, $" last: {input.Last}");
            if (input.OrderBy != null) arguments = string.Concat(arguments, $" orderBt: {input.OrderBy}");
            if (input.Stage != null) arguments = string.Concat(arguments, $" stage: {input.Stage}");
            if (input.Tags != null) arguments = string.Concat(arguments, $" where:{{tags_contains_all: [\"{string.Join("\", \"", input.Tags)}\"]}}");
            arguments = string.Concat(arguments, ")");
        }
        
        var request = new GraphQLRequest
        {
            Query = $@"
                    query {{
                      blogPostsConnection{arguments ?? null} {{
                        edges {{
                            node {{
                                id
                                title
                                content
                                stage
                                tags
                                image {{
                                  id
                                  url
                                  mimeType
                                  size
                                  width
                                  height
                                  fileName
                                }}
                              }}
                            }}
                        pageInfo{{
                            hasNextPage
                            hasPreviousPage
                            startCursor
                            endCursor
                            pageSize
                        }}
                    }}
            }}"
        };
        
        client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.HygraphSecret);

        var response = await client.SendQueryAsync<JsonElement>(request);

        if (response.Errors != null && response.Errors?.Length != 0)

        {
            throw new Exception(response.Errors?[0].Message);
        }

        var blogPosts = JsonUtil.Deserialize<BlogPostConnection>(response.Data.ToString(), ignoreRoot: true);

        return blogPosts!;
    }
    
    public async Task<Asset> UploadAssetAsync(IFile file)
    {
        var client = new HttpClient();
        
        var form = new MultipartFormDataContent();
        
        var fileContent = new StreamContent(file.OpenReadStream());
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
        form.Add(fileContent, "fileUpload", file.Name);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.HygraphSecret);
        
        var response = await client.PostAsync(_settings.HygraphAssetUploadUrl, form);

        var content = await response.Content.ReadAsStringAsync();

        var asset = JObject.Parse(content).ToObject<Asset>();

        if (response.StatusCode != HttpStatusCode.OK || asset == null)
        {
            throw new Exception(response.ReasonPhrase);
        }

        return asset;
    }
    
    public async Task<bool> PublishAsync(Type type, string id)
    {
        var client = new GraphQLHttpClient(new Uri(_settings.HygraphContentApiUrl), new SystemTextJsonSerializer());
        var request = new GraphQLRequest
        {
            Query = $@"
                    mutation {{
                        publish{type}(where: {{id: ""{id}""}}){{
                            id
                        }}
                    }}"
        };

        client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.HygraphSecret);

        var response = await client.SendQueryAsync<JsonElement>(request);
        
        if (response.Errors != null && response.Errors?.Length != 0)
        {
            throw new Exception(response.Errors?[0].Message);
        }

        return true;
    }
    
    public async Task<bool> UnpublishAsync(Type type, string id)
    {
        var client = new GraphQLHttpClient(new Uri(_settings.HygraphContentApiUrl), new SystemTextJsonSerializer());
        var request = new GraphQLRequest
        {
            Query = $@"
                    mutation {{
                        unpublish{type}(where: {{id: ""{id}""}}){{
                            id
                        }}
                    }}"
        };

        client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.HygraphSecret);

        var response = await client.SendQueryAsync<JsonElement>(request);
        
        if (response.Errors != null && response.Errors?.Length != 0)
        {
            throw new Exception(response.Errors?[0].Message);
        }

        return true;
    }
}
