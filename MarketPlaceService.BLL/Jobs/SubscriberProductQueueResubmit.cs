using MarketPlaceService.BLL.Contracts.Jobs;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketPlaceService.BLL.Jobs
{
    public class SubscriberProductQueueResubmit : IResubmitJob
    {
        private readonly IJobRepository _jobRepository;

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

        public SubscriberProductQueueResubmit(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public ResubmitJobResponse ResubmitJob(ResubmitJobRequest request)
        {
            ResubmitJobResponse response = new ResubmitJobResponse { IsSuccess = true };
            try
            {
                if (request.IsHistory)
                {
                   
                    _jobRepository.InsertSubscriberProductQueueData(Guid.Parse(request.JobId), _traceId);
                }
                else
                {
                    var bookingUpdateFromPublisherQueue = _jobRepository.UpdateSubscriberProductQueueData(Guid.Parse(request.JobId));
                }

                _jobRepository.SaveChanges();
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Error = new Entities.Error
                {
                    ErrorMessage = e.ToString()
                };
            }

            return response;
        }

        public ValidateResubmitJobResponse ValidateResubmitJob(ResubmitJobRequest request)
        {
            ValidateResubmitJobResponse response = new ValidateResubmitJobResponse
            {
                IsSuccess = true
            };

            if (!request.IsHistory)
                return response;

            var historyItem = _jobRepository.getSubscriberProductQueueHistoryItem(Guid.Parse(request.JobId));
            var queueItems = _jobRepository.GetSubscriberProductQueueItems();

            if (queueItems.Any(a => a.Subscriberproductid == historyItem.Subscriberproductid))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Cannot insert into queue. Subscriber Product Id already exists in the queue.";
            }

            return response;
        }
    }
}
