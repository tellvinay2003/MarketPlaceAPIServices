using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace MarketPlaceService.Entities.Job
{
    public static class BusinessProcessQueue
    {

        public static Dictionary<BusinessProcess, Type> QueueMapping = new Dictionary<BusinessProcess, Type>()
        {
            {BusinessProcess.PublishingToMarketplace, typeof(PublishingToMarketplace)},
            {BusinessProcess.SubscribingInMarketplace, typeof(SubscribingInMarketplace)},
            {BusinessProcess.SubscribingAtTheSubscriber, typeof(SubscribingAtTheSubscriber)},
            {BusinessProcess.NewBookingInTheSubscriber, typeof(NewBookingInTheSubscriber)},
            {BusinessProcess.UpdateBookingInTheSubscriber, typeof(UpdateBookingInTheSubscriber)},
            {BusinessProcess.UpdateBookingInThePublisher, typeof(UpdateBookingInThePublisher)},

        };

        public enum PublishingToMarketplace
        {
            [Description("New Product")]
            NewProduct = 1,
            [Description("Product Updates")]
            ProductUpdates
        }

        public enum SubscribingInMarketplace
        {
            [Description("New Product")]
            NewProduct = 1,
            [Description("Product Updates")]
            ProductUpdates,
            [Description("Unsubscribe")]
            Unsubscribe
        }

        public enum SubscribingAtTheSubscriber
        {
            [Description("Import New Product")]
            ImportNewProduct = 1,
            [Description("Import Product Updates")]
            ImportProductUpdates,
            [Description("Unsubscribe")]
            Unsubscribe,
            [Description("Confirm Product Import")]
            ConfirmProductImport,
            [Description("Confirm Product Updates Import")]
            ConfirmProductUpdatesImport,

        }

        public enum NewBookingInTheSubscriber
        {
            [Description("New Subscriber Booking")]
            NewSubscriberBooking = 1,
            [Description("Import Subscriber Booking")]
            ImportSubscriberBooking,
            [Description("Push Publisher Booking")]
            PushPublisherBooking,
            [Description("Import Publisher Booking")]
            ImportPublisherBooking,
            [Description("Confirm Publisher Booking Import")]
            ConfirmPublisherBookingImport,
            [Description("Update Subscriber Booking")]
            UpdateSubscriberBooking,

        }

        public enum UpdateBookingInTheSubscriber
        {
            [Description("Subscriber Booking Updates")]
            SubscriberBookingUpdates = 1,
            [Description("PushPublisherBookingUpdates")]
            PushPublisherBookingUpdates,
            [Description("Import Publisher Booking Updates")]
            ImportPublisherBookingUpdates,
            [Description("Confirm Publisher Booking Updates")]
            ConfirmPublisherBookingUpdates,
            [Description("Update Subscriber Booking")]
            UpdateSubscriberBooking,

        }

        public enum UpdateBookingInThePublisher
        {
            [Description("Publisher Booking Updates")]
            PublisherBookingUpdates = 1,
            [Description("Confirm Publisher Booking Updates")]
            ConfirmPublisherBookingUpdates,
            [Description("Update Subscriber Booking")]
            UpdateSubscriberBooking,

        }
    }

}
