using System;

namespace MarketPlaceService.Entities
{
    public class PublisherAgentMap
    {
        public Guid Id { get; set; }
        public Guid PublisherId { get; set; }
        public Guid SubscriberId { get; set; }
        public int AgentId { get; set; }        
        public int? OrganisationId {get;set;}
        public int? UserId {get;set;}
        public int? BookingPrefixId {get;set;}
    }
}
