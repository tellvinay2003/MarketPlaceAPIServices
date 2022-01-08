using System;

namespace MarketPlaceService.Entities
{
    public enum JobStatus
    {
        Queued = 1,
        Processing = 2,
        Retry = 3,
        Dormant = 4
    };

}
