using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class BookingSearchResponse:ResponseBase
    {
       public IEnumerable<BookingModel> Bookings {get;set;}
    }
}
