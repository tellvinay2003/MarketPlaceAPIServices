using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class Response<T>
    {
        //public int ResponseCode { get; set; }
        //public string Message { get; set; }
        //public T ResponseMessage { get; set; }
        //public string ErrorMessage { get; set; }

        public int ResponseCode { get; set; }

        public string Status { get; set; }
        public string Message { get; set; }
        public T ResponseMessage { get; set; }

        public long ExecutionTimeMS { get; set; }

        public Guid TraceId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
