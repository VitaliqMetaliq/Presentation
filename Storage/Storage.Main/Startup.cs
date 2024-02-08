using ExchangeTypes;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Storage.Core.Caching;
using Storage.Core.Consumers;
using Storage.Core.Managers;
using Storage.Core.Reports;
using Storage.Core.Settings;
using Storage.Core.StateMachines;
using Storage.Database;
using Storage.Database.Repository;
using Storage.Main.Extensions;
using Storage.Main.Managers;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Storage.Main;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureSettings(Configuration, out AppSettings appSettings);
        services.AddControllers();
        services.AddFluentValidation(options => options.RegisterValidatorsFromAssembly(this.GetType().Assembly));
        services.AddDbContext<StorageDbContext>(options =>
        {
            options.UseNpgsql(appSettings.ConnectionString);
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Storage.Main v1",
                Version = "v1"
            });
        });

        services.AddScoped<IBaseDataRepository, BaseDataRepository>();
        services.AddScoped<IDailyDataRepository, DailyDataRepository>();
        services.AddScoped<IStorageManager, StorageManager>();
        services.AddScoped<IReportManager, ReportManager>();
        services.AddScoped<IRepositoryAggregateManager, RepositoryAggregateManager>();
        services.AddSingleton<ICacheService, RedisCacheService>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetValue<string>("RedisConnection");
        });

        //services.AddSingleton<IConnectionMultiplexer>(x =>
        //    ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection")));

        services.ConfigureMassTransit(Configuration, new MassTransitConfiguration()
        {
            Configurator = bus =>
            {
                bus.AddConsumer<BaseDataStorageConsumer>();
                bus.AddSagaStateMachine<StorageStateMachine, StorageState>().InMemoryRepository();
            }
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Storage.Main";
            options.SwaggerEndpoint("v1/swagger.json", "Storage.Main");
            options.DocExpansion(DocExpansion.List);
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

