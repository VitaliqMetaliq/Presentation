using Converter.Core.Services;
using ExchangeTypes.Models;
using MassTransit;

namespace Converter.Core.Consumers;

public class ConverterConsumer : IConsumer<ConvertDataRequest>
{
    private readonly IConverterService _converterService;

    public ConverterConsumer(IConverterService converterService)
    {
        _converterService = converterService;
    }

    public async Task Consume(ConsumeContext<ConvertDataRequest> context)
    {
        if (context?.Message != null)
        {
            try
            {
                var result = _converterService.ConvertCurrencyRelations(context.Message.DailyCurrencyModels);
                await context.Publish<SaveDailyDataRequest>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    Date = context.Message.Date,
                    CurrencyRelations = result
                });
            }
            catch (Exception)
            {
                await context.Publish<FailedProcessingMessage>(new
                {
                    CorrelationId = context.Message.CorrelationId
                });
            }
        }
    }
}

