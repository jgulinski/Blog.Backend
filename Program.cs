using Blog.Backend;
using Blog.Backend.GraphQL;
using Blog.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

var settings = new Settings();
builder.Configuration.Bind(nameof(Settings), settings);
settings.Validate();

builder.Services.AddScoped<IStorageService, HygraphService>(_ => new HygraphService(settings));
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<UploadType>();

var app = builder.Build();
app.MapGraphQL();
app.Run();