using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Job
{
    public class ValidateResubmitJobResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
