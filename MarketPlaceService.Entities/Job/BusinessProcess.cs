using System;
using System.ComponentModel;

namespace MarketPlaceService.Entities.Job
{

    public enum BusinessProcess
    {
        [Description("Publishing to Marketplace")]
        PublishingToMarketplace = 1,
        [Description("Subscribing in Marketplace")]
        SubscribingInMarketplace,
        [Description("Subscribing at the Subscriber")]
        SubscribingAtTheSubscriber,
        [Description("New Booking in the Subscriber")]
        NewBookingInTheSubscriber,
        [Description("Update Booking in the Subscriber")]
        UpdateBookingInTheSubscriber,
        [Description("Update Booking in the Publisher")]
        UpdateBookingInThePublisher
    }
}
