using MarketPlaceService.Entities.Job;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.BLL.Contracts.Jobs
{
    public interface IResubmitJob
    {
        ValidateResubmitJobResponse ValidateResubmitJob(ResubmitJobRequest request);

        ResubmitJobResponse ResubmitJob(ResubmitJobRequest request);
    }
}
