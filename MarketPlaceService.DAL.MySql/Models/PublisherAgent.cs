using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublisherAgent
    {
        public Guid PublisherAgentId { get; set; }
        public Guid PublisherId { get; set; }
        public Guid SubscriberD { get; set; }
        public int AgentId { get; set; }
        public int? OrganisationId { get; set; }
        public int? UserId { get; set; }
        public int? BookingPrefixId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual Subscriber SubscriberDNavigation { get; set; }
    }
}
