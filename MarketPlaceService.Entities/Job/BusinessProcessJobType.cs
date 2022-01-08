using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace MarketPlaceService.Entities.Job
{
    public static class BusinessProcessJobType
    {
        public static Dictionary<Type,Type> jobTypeList = new Dictionary<Type, Type>()
        {
            {typeof(BusinessProcessQueue.PublishingToMarketplace), typeof(PublishingToMarketplace)},
            {typeof(BusinessProcessQueue.SubscribingInMarketplace), typeof(SubscribingInMarketplace)},
            {typeof(BusinessProcessQueue.SubscribingAtTheSubscriber), typeof(SubscribingAtTheSubscriber)},
            {typeof(BusinessProcessQueue.NewBookingInTheSubscriber), typeof(NewBookingInTheSubscriber)},
            {typeof(BusinessProcessQueue.UpdateBookingInTheSubscriber), typeof(UpdateBookingInTheSubscriber)},
            {typeof(BusinessProcessQueue.UpdateBookingInThePublisher), typeof(UpdateBookingInThePublisher)},
        };
        
        
                public static Dictionary<Enum,List<Enum>> QueueJobTypeMapping = new Dictionary<Enum, List<Enum>>()
        {
          
             {BusinessProcessQueue.PublishingToMarketplace.NewProduct,new List<Enum>{BusinessProcessJobType.PublishingToMarketplace.PublishNewProduct}},
             {BusinessProcessQueue.PublishingToMarketplace.ProductUpdates,new List<Enum>{BusinessProcessJobType.PublishingToMarketplace.DetectProductUpdates,BusinessProcessJobType.PublishingToMarketplace.PublishProductUpdates,BusinessProcessJobType.PublishingToMarketplace.PublishUpdatedProduct,BusinessProcessJobType.PublishingToMarketplace.PublishServiceTypeOptionUpdates,BusinessProcessJobType.PublishingToMarketplace.PublishServiceTypeFacilityUpdates,BusinessProcessJobType.PublishingToMarketplace.PublishServiceTypeExtraUpdates}},
    
             {BusinessProcessQueue.SubscribingInMarketplace.NewProduct,new List<Enum>{BusinessProcessJobType.SubscribingInMarketplace.SubscribeNewProduct}},
             {BusinessProcessQueue.SubscribingInMarketplace.ProductUpdates,new List<Enum>{BusinessProcessJobType.SubscribingInMarketplace.SubscribeProductUpdates,BusinessProcessJobType.SubscribingInMarketplace.SubscribeServiceTypeOptionUpdates,BusinessProcessJobType.SubscribingInMarketplace.SubscribeServiceTypeFacilityUpdates,BusinessProcessJobType.SubscribingInMarketplace.SubscribeServiceTypeExtraUpdates}},
             {BusinessProcessQueue.SubscribingInMarketplace.Unsubscribe,new List<Enum>{BusinessProcessJobType.SubscribingInMarketplace.UnsubscribeProduct}},
    
    
             {BusinessProcessQueue.SubscribingAtTheSubscriber.ImportNewProduct,new List<Enum>{BusinessProcessJobType.SubscribingAtTheSubscriber.ImportNewProduct}},
             {BusinessProcessQueue.SubscribingAtTheSubscriber.ImportProductUpdates,new List<Enum>{BusinessProcessJobType.SubscribingAtTheSubscriber.ImportProductUpdates,BusinessProcessJobType.SubscribingAtTheSubscriber.ImportServiceTypeOptionUpdates,BusinessProcessJobType.SubscribingAtTheSubscriber.ImportServiceTypeExtraUpdates,BusinessProcessJobType.SubscribingAtTheSubscriber.ImportServiceTypeFacilityUpdates}},
             {BusinessProcessQueue.SubscribingAtTheSubscriber.Unsubscribe,new List<Enum>{BusinessProcessJobType.SubscribingAtTheSubscriber.UnsubscribeProduct}},
             {BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductImport,new List<Enum>{BusinessProcessJobType.SubscribingAtTheSubscriber.ConfirmProductImport}},             
             {BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductUpdatesImport,new List<Enum>{BusinessProcessJobType.SubscribingAtTheSubscriber.ConfirmProductUpdatesImport}},


             
            {BusinessProcessQueue.NewBookingInTheSubscriber.NewSubscriberBooking,new List<Enum>{BusinessProcessJobType.NewBookingInTheSubscriber.DetectNewBooking}},
            {BusinessProcessQueue.NewBookingInTheSubscriber.ImportSubscriberBooking,new List<Enum>{BusinessProcessJobType.NewBookingInTheSubscriber.ImportNewBooking}},
            {BusinessProcessQueue.NewBookingInTheSubscriber.PushPublisherBooking,new List<Enum>{BusinessProcessJobType.NewBookingInTheSubscriber.SendNewBooking}},
            {BusinessProcessQueue.NewBookingInTheSubscriber.ImportPublisherBooking,new List<Enum>{BusinessProcessJobType.NewBookingInTheSubscriber.ImportNewBooking}},
            {BusinessProcessQueue.NewBookingInTheSubscriber.ConfirmPublisherBookingImport,new List<Enum>{BusinessProcessJobType.NewBookingInTheSubscriber.ConfirmBookingImport}},
            {BusinessProcessQueue.NewBookingInTheSubscriber.UpdateSubscriberBooking,new List<Enum>{BusinessProcessJobType.NewBookingInTheSubscriber.UpdateSubscriberBooking}},


            {BusinessProcessQueue.UpdateBookingInTheSubscriber.SubscriberBookingUpdates,new List<Enum>{BusinessProcessJobType.UpdateBookingInTheSubscriber.DetectBookingUpdates,BusinessProcessJobType.UpdateBookingInTheSubscriber.PublishBookingUpdates,BusinessProcessJobType.UpdateBookingInTheSubscriber.BookingUpdates}},
            {BusinessProcessQueue.UpdateBookingInTheSubscriber.PushPublisherBookingUpdates,new List<Enum>{BusinessProcessJobType.UpdateBookingInTheSubscriber.SendBookingUpdates}},
            {BusinessProcessQueue.UpdateBookingInTheSubscriber.ImportPublisherBookingUpdates,new List<Enum>{BusinessProcessJobType.UpdateBookingInTheSubscriber.ImportBookingUpdates}},
            {BusinessProcessQueue.UpdateBookingInTheSubscriber.ConfirmPublisherBookingUpdates,new List<Enum>{BusinessProcessJobType.UpdateBookingInTheSubscriber.ConfirmBookingUpdates}},
            {BusinessProcessQueue.UpdateBookingInTheSubscriber.UpdateSubscriberBooking,new List<Enum>{BusinessProcessJobType.UpdateBookingInTheSubscriber.UpdateSubscriberBooking}},            
        
            {BusinessProcessQueue.UpdateBookingInThePublisher.PublisherBookingUpdates,new List<Enum>{BusinessProcessJobType.UpdateBookingInThePublisher.DetectBookingUpdates,BusinessProcessJobType.UpdateBookingInThePublisher.PublishBookingUpdates}},
            {BusinessProcessQueue.UpdateBookingInThePublisher.ConfirmPublisherBookingUpdates,new List<Enum>{BusinessProcessJobType.UpdateBookingInThePublisher.ConfirmBookingUpdates}},
            {BusinessProcessQueue.UpdateBookingInThePublisher.UpdateSubscriberBooking,new List<Enum>{BusinessProcessJobType.UpdateBookingInThePublisher.UpdateSubscriberBooking}},
            

        


        };

        /*  public static Dictionary<Enum,List<Enum> DbJobTypeMapping = new Dictionary<Enum, List<Enum>>()
        {
           

        }; */

        
         public enum PublishingToMarketplace
        {
            [DbJobType(JobType.Product)]
            [Description("Publish New Product")]
            PublishNewProduct = 1,
            [DbJobType(JobType.Product)]
            [Description("Detect Product Updates")]
            
            DetectProductUpdates,
            [DbJobType(JobType.Product)]
            [Description("Publish Updated Product")]
            PublishUpdatedProduct,
            [DbJobType(JobType.Product)]
            [Description("Publish Product Updates")]
            PublishProductUpdates,
             [DbJobType(JobType.ServiceTypeOption)]
            [Description("Publish Service Type Option Updates")]
            PublishServiceTypeOptionUpdates,
             [DbJobType(JobType.ServiceTypeExtra)]
            [Description("Publish Service Type Extra Updates")]
            PublishServiceTypeExtraUpdates,
            [DbJobType(JobType.ServiceTypeFacility)]
            [Description("Publish Service Type Facility Updates")]
            PublishServiceTypeFacilityUpdates,
        }

         public enum SubscribingInMarketplace
        {   [DbJobType(JobType.Product)]
            [Description("Subscribe New Product")]
            SubscribeNewProduct=1,
            [DbJobType(JobType.Product)]
            [Description("Subscribe Product Updates")]
            SubscribeProductUpdates,
            [DbJobType(JobType.ServiceTypeOption)]
            [Description("Subscribe Service Type Option Updates")]
            SubscribeServiceTypeOptionUpdates,
            [DbJobType(JobType.ServiceTypeExtra)]
            [Description("Subscribe Service Type Extra Updates")]
            SubscribeServiceTypeExtraUpdates,
            [DbJobType(JobType.ServiceTypeFacility)]
            [Description("Subscribe Service Type Facility Updates")]
            SubscribeServiceTypeFacilityUpdates,
            [DbJobType(JobType.DisableProduct)]
            [Description("Unsubscribe Product")]
            UnsubscribeProduct
        }




        public enum SubscribingAtTheSubscriber

        {
            [DbJobType(JobType.Product)]
            [Description("Import New Product")]
            ImportNewProduct=1,
            [DbJobType(JobType.Product)]
            [Description("Import Product Updates")]
            ImportProductUpdates,
            [DbJobType(JobType.ServiceTypeOption)]
            [Description("Import Service Type Option Updates")]
            ImportServiceTypeOptionUpdates,
            [DbJobType(JobType.ServiceTypeExtra)]
            [Description("Import Service Type Extra Updates")]
            ImportServiceTypeExtraUpdates,
            [DbJobType(JobType.ServiceTypeFacility)]
            [Description("Import Service Type Facility Updates")]
            ImportServiceTypeFacilityUpdates,
            [DbJobType(JobType.DisableProduct)]
            [Description("Unsubscribe Product")]
            UnsubscribeProduct,
            [DbCallBackJobType(CallbackJobType.ProductImport)]
            [Description("Confirm Product Import")]
            ConfirmProductImport,
             [DbCallBackJobType(CallbackJobType.ProductImport)]
             [Description("Confirm Product Updates Import")]
            ConfirmProductUpdatesImport
             
        }





        


        public enum NewBookingInTheSubscriber
        {
            [DbCallBackJobType(CallbackJobType.BookingPush)]
            [Description("Detect New Booking")]
            DetectNewBooking=1,            
            [Description("Import New Booking")]
            ImportNewBooking,
            [Description("Send New Booking")]
            SendNewBooking,
            [DbCallBackJobType(CallbackJobType.BookingPush)]
            [Description("Confirm Booking Import")]
            ConfirmBookingImport,
            [DbCallBackJobType(CallbackJobType.SubscriberCallBack)]
            [Description("Update Subscriber Booking")]
            UpdateSubscriberBooking            
             
        }




    public enum UpdateBookingInTheSubscriber
            {
                
                [DbJobTypeId(11)]
                [Description("Detect Booking Updates")]//11
                DetectBookingUpdates=1,
                [DbCallBackJobType(CallbackJobType.BookingUpdate)]
                [Description("Publish Booking Updates")]
                PublishBookingUpdates,

                [Description("Booking Updates")]
                BookingUpdates,
                [Description("Send Booking Updates")]
                SendBookingUpdates,
                [Description("Import Booking Updates")]
                ImportBookingUpdates,
                [DbCallBackJobType(CallbackJobType.BookingUpdate)]
                [Description("Confirm Booking Updates")]
                ConfirmBookingUpdates,
                [DbCallBackJobType(CallbackJobType.SubscriberCallBack)]
                [Description("Update Subscriber Booking")]
                UpdateSubscriberBooking,

            }



    public enum UpdateBookingInThePublisher
            {
                [DbJobTypeId(13)]
                [Description("Detect Booking Updates")]//13
                DetectBookingUpdates=1,
                [DbCallBackJobType(CallbackJobType.CDCBookingUpdate)]
                [Description("Publish Booking Updates")]
                PublishBookingUpdates,
               [DbCallBackJobType(CallbackJobType.CDCBookingUpdate)]
                [Description("Confirm Booking Updates")]
                ConfirmBookingUpdates,
                [DbCallBackJobType(CallbackJobType.SubscriberCallBack)]
                [Description("Update Subscriber Booking")]
                UpdateSubscriberBooking,

            }

    }
}
