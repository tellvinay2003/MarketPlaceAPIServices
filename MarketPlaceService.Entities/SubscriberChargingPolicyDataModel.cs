using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class SubscriberChargingPolicyDataModel
    {
        public Guid SubscriberChargingPolicyID	{get;set;}

        public Guid SubscriberId {get;set;}

        public CommunicationType DefaultCommunicationType { get; set; }

        public List<ServiceTypeTypeChargingPolicy> DefaultChargingPolicy { get; set; }
        
    }

    public class CommunicationType{
        public int? Id { get; set; }
        public string  Name { get; set; }
    }

    public class ServiceTypeTypeChargingPolicy
    {
        public int ServiceTypeTypeId {get;set;}

        public int? OptionChargingPolicyId {get;set;}

        public int? ExtraChargingPolicyId {get;set;}
    }
}
