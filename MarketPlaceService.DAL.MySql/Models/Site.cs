using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class Site
    {
        public Site()
        {
            BookingUpdateFromPublisherQueue = new HashSet<BookingUpdateFromPublisherQueue>();
            BookingUpdateFromPublisherQueueHistory = new HashSet<BookingUpdateFromPublisherQueueHistory>();
            MappingData = new HashSet<MappingData>();
            MarketplaceBooking = new HashSet<MarketplaceBooking>();
            MarketplaceBookingHistory = new HashSet<MarketplaceBookingHistory>();
            MarketplaceBookingPushQueue = new HashSet<MarketplaceBookingPushQueue>();
            MarketplaceBookingPushQueueHistory = new HashSet<MarketplaceBookingPushQueueHistory>();
            Publisher = new HashSet<Publisher>();
            SiteBooking = new HashSet<SiteBooking>();
            SiteBookingHistory = new HashSet<SiteBookingHistory>();
            SiteBookingPushQueueHistoryPublishersite = new HashSet<SiteBookingPushQueueHistory>();
            SiteBookingPushQueueHistorySubscribersite = new HashSet<SiteBookingPushQueueHistory>();
            SiteBookingPushQueuePublishersite = new HashSet<SiteBookingPushQueue>();
            SiteBookingPushQueueSubscribersite = new HashSet<SiteBookingPushQueue>();
            StaticDataUpdateQueue = new HashSet<StaticDataUpdateQueue>();
            Subscriber = new HashSet<Subscriber>();
        }

        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<BookingUpdateFromPublisherQueue> BookingUpdateFromPublisherQueue { get; set; }
        public virtual ICollection<BookingUpdateFromPublisherQueueHistory> BookingUpdateFromPublisherQueueHistory { get; set; }
        public virtual ICollection<MappingData> MappingData { get; set; }
        public virtual ICollection<MarketplaceBooking> MarketplaceBooking { get; set; }
        public virtual ICollection<MarketplaceBookingHistory> MarketplaceBookingHistory { get; set; }
        public virtual ICollection<MarketplaceBookingPushQueue> MarketplaceBookingPushQueue { get; set; }
        public virtual ICollection<MarketplaceBookingPushQueueHistory> MarketplaceBookingPushQueueHistory { get; set; }
        public virtual ICollection<Publisher> Publisher { get; set; }
        public virtual ICollection<SiteBooking> SiteBooking { get; set; }
        public virtual ICollection<SiteBookingHistory> SiteBookingHistory { get; set; }
        public virtual ICollection<SiteBookingPushQueueHistory> SiteBookingPushQueueHistoryPublishersite { get; set; }
        public virtual ICollection<SiteBookingPushQueueHistory> SiteBookingPushQueueHistorySubscribersite { get; set; }
        public virtual ICollection<SiteBookingPushQueue> SiteBookingPushQueuePublishersite { get; set; }
        public virtual ICollection<SiteBookingPushQueue> SiteBookingPushQueueSubscribersite { get; set; }
        public virtual ICollection<StaticDataUpdateQueue> StaticDataUpdateQueue { get; set; }
        public virtual ICollection<Subscriber> Subscriber { get; set; }
    }
}
