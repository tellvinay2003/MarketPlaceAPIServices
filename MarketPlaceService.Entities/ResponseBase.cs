using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class ResponseBase
    {
        public bool IsSuccess { get; set; }
        public List<Error> Errors{ get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public short ErrorId { get; set; }
        public ErrorType Type { get; set; }
    }
}
