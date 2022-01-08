using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IQueuedUpdateService
    {
        Task<IEnumerable<SubscriberProductTsUpdateQueue>> GetQueuedUpdates(int limit,string serverName);
        Task<SubscriberProductTsUpdateQueue> GetQueuedUpdate(Guid id);
        Task<SubscriberProductTsUpdateQueue> InsertQueuedUpdate(SubscriberProductTsUpdateQueue request);
        Task<SubscriberProductTsUpdateQueue> UpdateQueuedUpdate(Guid id,SubscriberProductTsUpdateQueue request);
        Task<bool> DeleteQueuedUpdate(Guid id);
        Task<SubscriberProductTsUpdateQueue> InsertQueuedUpdateHistory(SubscriberProductTsUpdateQueue request);

        Guid TraceId { get; set; }
    }
}
