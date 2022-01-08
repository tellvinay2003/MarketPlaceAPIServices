using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Job
{
    public class ResubmitJobResponse
    {
        public Error Error { get; set; }
        public string JobId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
