using System;

namespace MarketPlaceService.Entities
{
    public class BookingModel
    {

       public string BookingReference{get;set;}
       public Guid BookingId{get;set;} // can be sitebookingid or mp booking id
       public string BookingName{get;set;}
       public string BookingStatusName{get;set;}

       public int? BookingStatusId{get;set;}
       public DateTime? BookingDate{get;set;} // for the popup data

    }
}
