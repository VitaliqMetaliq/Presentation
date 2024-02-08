using ExchangeTypes.Models;
using MassTransit;
using Storage.Core.Managers;

namespace Storage.Core.StateMachines;

public class StorageStateMachine : MassTransitStateMachine<StorageState>
{
    public StorageStateMachine(IRepositoryAggregateManager repositoryManager)
    {
        InstanceState(e => e.CurrentState);
        Event(() => SaveBulkDataEvent, 
            x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => SaveDailyDataEvent,
            x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => FailedProcessingEvent,
            x => x.CorrelateById(y => y.Message.CorrelationId));
        Initially(When(SaveBulkDataEvent).ThenAsync(async e =>
        {
            try
            {
                e.Saga.ProcessingDates = e.Message.Dates;
                var earliestDate = FindEarliestDate(e.Saga.ProcessingDates);
                e.Saga.InitialDate = earliestDate.AddDays(-1);
                await e.Publish<ProcessDailyDataRequest>(new
                {
                    CorrelationId = e.Message.CorrelationId,
                    Date = earliestDate
                });
            }
            catch (Exception)
            {
                await e.Publish<FailedProcessingMessage>(new
                {
                    CorrelationId = e.Message.CorrelationId
                });
            }

        }).TransitionTo(InProgress));

        During(Failed, Ignore(FailedProcessingEvent));

        During(InProgress,
            Ignore(SaveBulkDataEvent), 
            When(SaveDailyDataEvent).ThenAsync(async e =>
            {
                try
                {
                    if (e.Saga.ProcessingDates.Count > 0 && e.Saga.ProcessingDates.Contains(e.Message.Date))
                    {
                        e.Saga.ProcessingDates.Remove(e.Message.Date);
                        await repositoryManager.ConvertAndSaveDailyData(e.Message);
                    }

                    if (e.Saga.ProcessingDates.Count == 0)
                    {
                        await e.Publish<FinalizeSavingRequest>(new {e.Message.CorrelationId});
                    }
                    else
                    {
                        var earliestDate = FindEarliestDate(e.Saga.ProcessingDates);
                        await e.Publish<ProcessDailyDataRequest>(new { e.Message.CorrelationId, Date = earliestDate });
                    }
                }
                catch (Exception)
                {
                    await e.Publish<FailedProcessingMessage>(new
                    {
                        CorrelationId = e.Message.CorrelationId
                    });
                }
                
            }), 
            When(FailedProcessingEvent).ThenAsync(async e =>
            {
                await repositoryManager.DeleteLatestItems(e.Saga.InitialDate);
            }).TransitionTo(Failed),
            When(FinalizeCrawlingEvent).Finalize());
    }

    public State InProgress { get; private set; }
    public State Failed { get; private set; }

    public Event<SaveBulkDataRequest> SaveBulkDataEvent { get; set; }
    public Event<SaveDailyDataRequest> SaveDailyDataEvent { get; set; }
    public Event<FinalizeCrawlingRequest> FinalizeCrawlingEvent { get; set; }
    public Event<FailedProcessingMessage> FailedProcessingEvent { get; private set; }

    private static DateTime FindEarliestDate(IReadOnlyCollection<DateTime> dates)
    {
        var date = dates.First();
        foreach (var current in dates.Where(e => e < date))
        {
            date = current;
        }

        return date;
    }
}

public class StorageState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string? CurrentState { get; set; }
    public DateTime InitialDate { get; set; }
    public List<DateTime> ProcessingDates { get; set; } = new();
}
