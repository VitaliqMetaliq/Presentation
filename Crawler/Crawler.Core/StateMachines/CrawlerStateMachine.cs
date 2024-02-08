using Crawler.Core.Services;
using Crawler.Database.Repository;
using ExchangeTypes.Models;
using MassTransit;

namespace Crawler.Core.StateMachines;

public class CrawlerStateMachine : MassTransitStateMachine<CrawlerState>
{
    public CrawlerStateMachine(IHttpClientService httpClientService, ISavePointRepository repository)
    {
        Event(() => ProcessDailyDataEvent,
            e => e.CorrelateById(x => x.Message.CorrelationId));
        Event(() => FinalizeSavingEvent,
            e => e.CorrelateById(x => x.Message.CorrelationId));
        Event(() => FailedProcessingEvent,
            x => x.CorrelateById(y => y.Message.CorrelationId));
        InstanceState(x => x.CurrentState);
        Initially(When(ProcessDailyDataEvent)
            .ThenAsync(async e =>
            {
                e.Saga.ProcessedDates.Add(e.Message.Date);

                var dailyData = await httpClientService.GetDailyCurrencyData(e.Message.Date);
                
                await e.Publish<ConvertDataRequest>(new
                {
                    CorrelationId =e.Message.CorrelationId, 
                    Date = e.Message.Date,
                    DailyCurrencyModels = dailyData
                });
            })
            .TransitionTo(InProgress));

        During(InProgress, 
            When(ProcessDailyDataEvent).ThenAsync(async e =>
            {
                e.Saga.ProcessedDates.Add(e.Message.Date);
                var dailyData = await httpClientService.GetDailyCurrencyData(e.Message.Date);

                await e.Publish<ConvertDataRequest>(new
                {
                    CorrelationId = e.Message.CorrelationId, 
                    Date = e.Message.Date, 
                    DailyCurrencyModels = dailyData
                });
            }),
            When(FailedProcessingEvent)
                .TransitionTo(Failed),
            When(FinalizeSavingEvent)
                .ThenAsync(async e =>
                {
                    var lastDate = GetLastDateFromState(e.Saga.ProcessedDates);
                    await repository.UpdateAsync(lastDate);
                    await e.Publish<FinalizeCrawlingRequest>(new
                    {
                        CorrelationId = e.Message.CorrelationId
                    });
                })
                .Finalize());
    }

    public State InProgress { get; private set; }
    public State Failed { get; private set; }

    public Event<ProcessDailyDataRequest> ProcessDailyDataEvent { get; private set; }
    public Event<FinalizeSavingRequest> FinalizeSavingEvent { get; private set; }
    public Event<FailedProcessingMessage> FailedProcessingEvent { get; private set; }

    private static DateTime GetLastDateFromState(IReadOnlyCollection<DateTime> collection)
    {
        var date = collection.First();
        foreach (var current in collection.Where(e => e > date))
        {
            date = current;
        }

        return date;
    }
}

public class CrawlerState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public List<DateTime> ProcessedDates { get; set; } = new();
}