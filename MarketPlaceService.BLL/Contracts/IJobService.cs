using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Job;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IJobService
    {
        Guid TraceId { get; set; }

        Task<List<BusinessProcessDataModel>> GetBusinessProcess();
        Task<List<JobStatusDataModel>> GetJobStatus(bool forCurrentJobs);

        Task<JobSearchResponse> SearchJob(JobSearchRequest request);

        Task<List<QueueDataModel>> GetBusinessProcessQueue(BusinessProcess businessProcess);

        Task<JobInfoResponse> JobInfo(JobInfoRequest request);
        Task<ResubmitJobResponse> ResubmitJob(ResubmitJobRequest request);
    }
}
