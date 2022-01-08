using System;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using MarketPlaceService.DAL.Contract;
using System.Net.Http;
using Newtonsoft.Json;
using MarketPlaceService.BLL.UtilityService;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Linq;
using MarketPlaceService.Utilities;
using CommonUtilities;

namespace MarketPlaceService.BLL
{
    public class QueuedUpdateService : IQueuedUpdateService
    {
        private readonly ILogger<QueuedUpdateService> _logger;

        private readonly IQueuedUpdatesRepository _queuedUpdatesRepository;

         private Guid _traceId;
        public Guid TraceId
        {
            get
            {
                if (_traceId == Guid.Empty)
                    _traceId = Guid.NewGuid();
                return _traceId;
            }
            set
            {
                _traceId = value;
            }
        }
        public QueuedUpdateService(IQueuedUpdatesRepository queuedUpdatesRepository,ILogger<QueuedUpdateService> logger)
        {
          _logger = logger;
          _queuedUpdatesRepository = queuedUpdatesRepository;
        }

        public async Task<IEnumerable<SubscriberProductTsUpdateQueue>> GetQueuedUpdates(int limit,string serverName)
        {
            //used Logtrace as this method gets called every second by adapter so by default this wont be logged
            LoggingHelper.LogTrace(_logger, LogType.Start, "GetQueuedUpdates", "QueuedUpdateService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _queuedUpdatesRepository.GetQueuedUpdates(limit,serverName);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetQueuedUpdates", "QueuedUpdateRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogTrace(_logger, LogType.End, "GetQueuedUpdates", "QueuedUpdateService", TraceId);
            return result;
        }

        public async Task<SubscriberProductTsUpdateQueue> GetQueuedUpdate(Guid id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetQueuedUpdate", "QueuedUpdateService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _queuedUpdatesRepository.GetQueuedUpdate(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetQueuedUpdate", "QueuedUpdateRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetQueuedUpdate", "QueuedUpdateService", TraceId);
            return result;
        }

        public async Task<SubscriberProductTsUpdateQueue> InsertQueuedUpdate(SubscriberProductTsUpdateQueue request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertQueuedUpdate", "QueuedUpdateService", TraceId);
            var watch = Stopwatch.StartNew();
            request.TraceId=TraceId;
            var result = new SubscriberProductTsUpdateQueue();

            if(request.CallBackJobTypeId == 0 || request.CallBackJobTypeId == (int)CallbackJobType.ProductImport) //productImport
            {
                result =  await _queuedUpdatesRepository.InsertSubscriberProductTsUpdateQueue(request);
            }
            else if(request.CallBackJobTypeId == (int)CallbackJobType.SubscriberCallBack) //subscriberCallBack --> need to confirm the id
            {
                result= await _queuedUpdatesRepository.InsertBookingUpdateQueueFromSubscriber(request);
            }
            else
            {
                result =  await _queuedUpdatesRepository.InsertBookingUpdateFromPublisherQueue(request);
            }

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertQueuedUpdate", "QueuedUpdateRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertQueuedUpdate", "QueuedUpdateService", TraceId);
            return result;
        }

        public async Task<SubscriberProductTsUpdateQueue> UpdateQueuedUpdate(Guid id,SubscriberProductTsUpdateQueue request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateQueuedUpdate", "QueuedUpdateService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _queuedUpdatesRepository.UpdateQueuedUpdate(id,request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateQueuedUpdate", "QueuedUpdateRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateQueuedUpdate", "QueuedUpdateService", TraceId);
            return result;
        }

        public async Task<bool> DeleteQueuedUpdate(Guid id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteQueuedUpdate", "QueuedUpdateService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _queuedUpdatesRepository.DeleteQueuedUpdate(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteQueuedUpdate", "QueuedUpdateRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteQueuedUpdate", "QueuedUpdateService", TraceId);
            return result;
        }

        public  async Task<SubscriberProductTsUpdateQueue> InsertQueuedUpdateHistory(SubscriberProductTsUpdateQueue request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertQueuedUpdateHistory", "QueuedUpdateService", TraceId);
            var watch = Stopwatch.StartNew();     
            request.TraceId=TraceId;       
            var result = await _queuedUpdatesRepository.InsertQueuedUpdateHistory(request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertQueuedUpdateHistory", "QueuedUpdateRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertQueuedUpdateHistory", "QueuedUpdateService", TraceId);
            return result;
        }
    }
}
