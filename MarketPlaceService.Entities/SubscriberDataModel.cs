using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class SubscriberDataModel
    {
       public Guid SubscriberId { get; set; }
        public Guid SiteId { get; set; }
        public int? OrganizationId { get; set; }
        public string SubscriberName { get; set; }
        public bool Enabled { get; set; }

        public string Message {get; set; }
        public bool IsValid {get;set;}
    }
}
