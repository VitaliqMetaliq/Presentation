using Crawler.Main.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();
builder.RegisterLogger();

var app = builder.Build();

await app.InitApplication();
app.ConfigurePipeline();

await app.RunAsync();
