using Crawler.Core.Caching;
using Crawler.Core.Services;
using Crawler.Core.Settings;
using Crawler.Database;
using ExchangeTypes;
using ExchangeTypes.Models;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Crawler.Core.StateMachines;
using Crawler.Database.Repository;
using MassTransit;

namespace Crawler.Main.Extensions;

internal static class WebApplicationExtensions
{
    internal static void RegisterLogger(this WebApplicationBuilder builder)
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();

        builder.Host.UseSerilog(logger);
    }

    internal static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureSettings(builder.Configuration, out AppSettings appSettings);
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        builder.Services.AddMemoryCache();
        builder.Services.AddDbContext<HangfireDbContext>(options =>
        {
            options.UseNpgsql(appSettings.ConnectionString, b => b.MigrationsAssembly("Crawler.Database"));
        });

        builder.Services.AddHangfire(h =>
        {
            h.UsePostgreSqlStorage(appSettings.ConnectionString);
        });
        builder.Services.AddHangfireServer();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Crawler.Main v1",
                Version = "v1"
            });
        });

        builder.Services.ConfigureMassTransit(builder.Configuration, new MassTransitConfiguration()
        {
            Configurator = bus =>
            {
                bus.AddRequestClient<ConvertDataRequest>();
                bus.AddRequestClient<SaveBaseDataRequest>();
                // bus.AddSagaStateMachine<CrawlerStateMachine, CrawlerState>().InMemoryRepository();
            }
        });

        builder.Services.AddScoped<IMemoryCacheManager, MemoryCacheManager>();
        builder.Services.AddScoped<IHttpClientService, HttpClientService>();
        builder.Services.AddScoped<ICrawlerJobManager, CrawlerJobManager>();
        builder.Services.AddScoped<ISavePointRepository, SavePointRepository>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    internal static void ConfigurePipeline(this WebApplication app)
    {
        app.UseRouting();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Crawler.Main";
            options.SwaggerEndpoint("v1/swagger.json", "Crawler.Main");
            options.DocExpansion(DocExpansion.List);
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHangfireDashboard();
        });
        app.UseHangfireDashboard();

        BackgroundJob.Enqueue<ICrawlerJobManager>(x => x.EnqueueFireAndForgetJob());
        RecurringJob.AddOrUpdate<ICrawlerJobManager>(x => x.EnqueueDailyJob(), Cron.Daily);
    }

    internal static async Task InitApplication(this WebApplication app)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

        try
        {
            Log.Debug("Starting web host");
            await app.InitDatabase();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

