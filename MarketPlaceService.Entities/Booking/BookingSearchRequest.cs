using System;
using MarketPlaceService.Entities;
namespace MarketPlaceService.Entities.Booking
{
    public class BookingSearchRequest
    {
        public Guid SiteId {get;set;}
        public Guid EntityId{get;set;} //subscriberid or publisherid
        public EntityType EntityType{get;set;}
        public string BookingReference{get;set;}
        public DateTime? FromDate{get;set;}
        public DateTime? ToDate{get;set;}
        public int? StatusId{get;set;}
    }
}
