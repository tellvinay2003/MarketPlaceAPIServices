using System;

namespace MarketPlaceService.Entities.Booking
{
    public class ProcessSubscriberCallbackRequest
    {
        public Guid SiteBookingId { get; set; }
        public bool IsProcessedSuccessfully { get; set; }
        public string JobNote { get; set; } // If failure on subscriber side, expected to return error note
    }
}
