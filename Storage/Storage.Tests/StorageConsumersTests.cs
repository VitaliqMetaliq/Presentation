using ExchangeTypes.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Storage.Core.Consumers;

namespace Storage.Tests;

public class StorageConsumersTests
{
    [Fact]
    public async Task BaseDataStorageConsumer_Test_PositiveCase()
    {
        await using var provider = new ServiceCollection().AddMassTransitTestHarness(cfg =>
        {
            cfg.AddConsumer<BaseDataStorageConsumer>();
        }).BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();

        await harness.Bus.Publish(new SaveBaseDataRequest()
        {
            BaseCurrencies = new List<CommonCurrencyModel>()
        });

        Assert.True(await harness.Consumed.Any<SaveBaseDataRequest>());
    }
}