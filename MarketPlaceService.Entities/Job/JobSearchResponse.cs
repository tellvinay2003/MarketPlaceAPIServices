using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities.Job
{
    public class JobSearchResponse
    {
        public List<JobRecord> JobRecords;

        public int TotalRecords{get {return JobRecords!=null?JobRecords.Count:0;}}

    }
}
