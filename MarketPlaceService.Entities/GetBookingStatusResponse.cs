using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class GetBookingStatusResponse
    {
       public IEnumerable<BookingStatusDataModel> BookingStatusList; 
    }
}
