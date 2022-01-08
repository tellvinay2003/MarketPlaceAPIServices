using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Contract
{
    public interface IQueuedUpdatesRepository
    {
        Task<IEnumerable<Entities.SubscriberProductTsUpdateQueue>> GetQueuedUpdates(int limit,string serverName);
        Task<Entities.SubscriberProductTsUpdateQueue> GetQueuedUpdate(Guid id);
        Task<Entities.SubscriberProductTsUpdateQueue> InsertSubscriberProductTsUpdateQueue(Entities.SubscriberProductTsUpdateQueue request);
        Task<Entities.SubscriberProductTsUpdateQueue> InsertBookingUpdateFromPublisherQueue(Entities.SubscriberProductTsUpdateQueue request);
        Task<Entities.SubscriberProductTsUpdateQueue> UpdateQueuedUpdate(Guid id,Entities.SubscriberProductTsUpdateQueue request);
        Task<bool> DeleteQueuedUpdate(Guid id);
        Task<Entities.SubscriberProductTsUpdateQueue> InsertQueuedUpdateHistory(Entities.SubscriberProductTsUpdateQueue request);
        Task<Entities.SubscriberProductTsUpdateQueue> InsertBookingUpdateQueueFromSubscriber(Entities.SubscriberProductTsUpdateQueue request);
    }
}
