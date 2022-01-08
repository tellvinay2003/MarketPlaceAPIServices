using System;
using System.ComponentModel;
namespace MarketPlaceService.Entities.Booking
{
    public enum BookingEvent
    {

        [Description("Booking : New ")]
        BookingNew = 1,
        [Description("Booking : Update")]
        BookingUpdate,
        [Description("Booking : Update Error")]
        BookingUpdateError
    }
}
