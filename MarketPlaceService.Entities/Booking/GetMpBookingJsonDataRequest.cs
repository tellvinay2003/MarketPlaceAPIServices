using System;

namespace MarketPlaceService.Entities.Booking
{
    public class GetMpBookingJsonDataRequest
    {
        
        public Guid? RowId{get;set;}
        public EntityType EntityType{get;set;} // publisher or subscriber
        public Guid? BookingId {get;set;} //marketplacebooking or sitebookingid
        public EntityType JsonDataType{get;set;}  // publisher

    }
}
