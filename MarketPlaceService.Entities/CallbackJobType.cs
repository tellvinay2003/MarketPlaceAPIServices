using System;

namespace MarketPlaceService.Entities
{
    public enum CallbackJobType
    {
        BookingPush = 1,
        BookingUpdate = 2,
        CDCBookingUpdate = 3,
        ProductImport = 4,
        SubscriberCallBack = 5,
    }
}
