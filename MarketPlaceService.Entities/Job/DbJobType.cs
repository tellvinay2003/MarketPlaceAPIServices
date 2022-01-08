using System;

namespace MarketPlaceService.Entities.Job
{
    public class DbJobType : Attribute
    {
        internal DbJobType(JobType jobType)
        {
            this.JobType = jobType;

        }
        public JobType JobType { get; private set; }
    }


    public class DbCallBackJobType : Attribute
    {
        internal DbCallBackJobType(CallbackJobType callBackJobType)
        {
            this.callBackJobType = callBackJobType;

        }
        public CallbackJobType callBackJobType { get; private set; }
    }



    public class DbJobTypeId : Attribute
    {
        internal DbJobTypeId(short dbJobTypeId)
        {
            this.dbJobTypeId = dbJobTypeId;

        }
        public short dbJobTypeId { get; private set; }
    }

}
