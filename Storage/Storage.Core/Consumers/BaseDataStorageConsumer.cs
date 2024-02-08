using ExchangeTypes.Models;
using MassTransit;
using Storage.Core.Mapping;
using Storage.Database.Entities;
using Storage.Database.Repository;

namespace Storage.Core.Consumers;

public class BaseDataStorageConsumer : IConsumer<SaveBaseDataRequest>
{
    private readonly IBaseDataRepository _baseDataRepository;

    public BaseDataStorageConsumer(IBaseDataRepository baseDataRepository)
    {
        _baseDataRepository = baseDataRepository;
    }

    public async Task Consume(ConsumeContext<SaveBaseDataRequest> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        SaveBaseDataRequest? request = context?.Message;
        if (request != null)
        {
            IEnumerable<BaseCurrencyEntity> existingItems = await _baseDataRepository.GetAllAsync();
            if (!existingItems.Any())
            {
                await _baseDataRepository.CreateBulkAsync(request.BaseCurrencies.Select(e => e.ToEntity()));
            }
            else
            {
                var result = request.BaseCurrencies.Select(e => e.ToEntity());
                foreach (var item in result)
                {
                    await _baseDataRepository.AddOrUpdateAsync(item);
                }
            }

            await _baseDataRepository.SaveChangesAsync();
        }
    }

    private (List<BaseCurrencyEntity> notExistingItems, List<BaseCurrencyEntity> itemsToUpdate) GetUpdatingData(
        IEnumerable<BaseCurrencyEntity> existingEntities, IEnumerable<CommonCurrencyModel> commonModels)
    {
        var notExistingItems = new List<BaseCurrencyEntity>();
        var itemsToUpdate = new List<BaseCurrencyEntity>();
        foreach (CommonCurrencyModel commonItem in commonModels)
        {
            bool isFound = false;
            foreach (BaseCurrencyEntity existingItem in existingEntities)
            {
                if (existingItem.ISOCharCode == commonItem.IsoCharCode)
                {
                    isFound = true;
                    itemsToUpdate.Add(commonItem.ToEntity(existingItem.Id));
                    break;
                }
            }

            if (!isFound)
            {
                notExistingItems.Add(commonItem.ToEntity());
            }
        }

        return (notExistingItems, itemsToUpdate);
    }
}
