using System;

namespace MarketPlaceService.Entities.Job
{
    public enum DbJobQueue
    {
        PUBLISHEDPRODUCTSQUEUE =1 ,
        SUBSCRIBERPRODUCTQUEUE,

        SUBSCRIBERPRODUCTTSUPDATEQUEUE,

        MARKETPLACEBOOKINGPUSHQUEUE,

        SITEBOOKINGPUSHQUEUE,

        BOOKINGUPDATEFROMPUBLISHERQUEUE,


    }
}