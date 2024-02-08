using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTypes;

public static class MassTransitServiceCollectionExtensions
{
    public static void ConfigureMassTransit(
        this IServiceCollection services,
        IConfiguration configuration,
        MassTransitConfiguration massTransitConfiguration)
    {
        var rabbitSection = configuration.GetSection("RabbitMqConfiguration");
        var url = rabbitSection.GetValue<string>("Url");
        var host = rabbitSection.GetValue<string>("Host");

        if (rabbitSection == null || string.IsNullOrEmpty(url) || string.IsNullOrEmpty(host))
        {
            throw new ArgumentException("AppSettings does not contain configuration for RabbitMQ");
        }

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(url, host, h =>
                {
                    h.Username(rabbitSection.GetValue<string>("Username"));
                    h.Password(rabbitSection.GetValue<string>("Password"));
                });

                cfg.ConfigureEndpoints(context);
            });

            massTransitConfiguration.Configurator?.Invoke(configurator);
        });
    }
}

