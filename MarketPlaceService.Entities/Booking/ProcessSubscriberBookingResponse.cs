using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class ProcessSubscriberBookingResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorText { get; set; }
    }
}
