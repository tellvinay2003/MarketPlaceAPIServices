using MarketPlaceService.BLL.Contracts.Jobs;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Job;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.BLL.Jobs
{
    public class BookingUpdateFromPublisherQueueResubmit : IResubmitJob
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

        public BookingUpdateFromPublisherQueueResubmit(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public ResubmitJobResponse ResubmitJob(ResubmitJobRequest request)
        {
            ResubmitJobResponse response = new ResubmitJobResponse
            {
                IsSuccess = true
            };
            try
            {
                if (request.IsHistory)
                {
                    _jobRepository.InsertBookingUpdateFromPublisherQueueData(Guid.Parse(request.JobId), _traceId);
                }
                else
                {
                    var bookingUpdateFromPublisherQueue = _jobRepository.UpdateBookingUpdateFromPublisherQueueData(Guid.Parse(request.JobId));
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

            return response;
        }
    }
}
