using Converter.Core.Consumers;
using Converter.Core.Services;
using ExchangeTypes;
using Microsoft.AspNetCore.Builder;

namespace Converter.Main.Extensions;

internal static class WebApplicationExtensions
{
    public static void ConfigureWebApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IConverterService, ConverterService>();
        builder.Services.ConfigureMassTransit(builder.Configuration, new MassTransitConfiguration
        {
            Configurator = bus =>
            {
                bus.AddConsumer<ConverterConsumer>();
            }
        });
    }
}

