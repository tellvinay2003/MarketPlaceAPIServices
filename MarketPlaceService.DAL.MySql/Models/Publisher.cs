using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class Publisher
    {
        public Publisher()
        {
            PublishedProducts = new HashSet<PublishedProducts>();
            PublishedProductsQueue = new HashSet<PublishedProductsQueue>();
            PublisherAgent = new HashSet<PublisherAgent>();
            PublisherDefault = new HashSet<PublisherDefault>();
            PublisherServiceStatus = new HashSet<PublisherServiceStatus>();
            PublisherSupplierStatus = new HashSet<PublisherSupplierStatus>();
            SiteBooking = new HashSet<SiteBooking>();
            SiteBookingHistory = new HashSet<SiteBookingHistory>();
            SiteBookingPublisher = new HashSet<SiteBookingPublisher>();
            SubscriberSupplier = new HashSet<SubscriberSupplier>();
        }

        public Guid PublisherId { get; set; }
        public Guid SiteId { get; set; }
        public int? OrganizationId { get; set; }
        public string PublisherName { get; set; }
        public bool Enabled { get; set; }

        public virtual Site Site { get; set; }
        public virtual ICollection<PublishedProducts> PublishedProducts { get; set; }
        public virtual ICollection<PublishedProductsQueue> PublishedProductsQueue { get; set; }
        public virtual ICollection<PublisherAgent> PublisherAgent { get; set; }
        public virtual ICollection<PublisherDefault> PublisherDefault { get; set; }
        public virtual ICollection<PublisherServiceStatus> PublisherServiceStatus { get; set; }
        public virtual ICollection<PublisherSupplierStatus> PublisherSupplierStatus { get; set; }
        public virtual ICollection<SiteBooking> SiteBooking { get; set; }
        public virtual ICollection<SiteBookingHistory> SiteBookingHistory { get; set; }
        public virtual ICollection<SiteBookingPublisher> SiteBookingPublisher { get; set; }
        public virtual ICollection<SubscriberSupplier> SubscriberSupplier { get; set; }
    }
}
