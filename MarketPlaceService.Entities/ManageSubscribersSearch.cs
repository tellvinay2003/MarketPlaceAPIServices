using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class ManageSubscribersSearch
    {
        public Guid PublisherId {get;set;}
        public Guid SubscriberId {get;set;}
        public List<int> ProductTypeId {get;set;}
        public int RegionId {get;set;}
        public string ProductName {get;set;}
        public int InCludeSubscriber{get;set;}
    }

    public enum InCludeSubscriber
    {
        Ignore=1,
        Include=2,
        Exclude=3
    }
}
