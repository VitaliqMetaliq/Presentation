using MassTransit;

namespace ExchangeTypes;

public class MassTransitConfiguration
{
    public Action<IBusRegistrationConfigurator> Configurator { get; set; }

    public Action<IBusControl, IServiceProvider> BusControl { get; set; }
}

