using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SiteBookingPublisher
    {
        public Guid Sitebookingpublisherid { get; set; }
        public Guid Sitebookingid { get; set; }
        public Guid? Publisherid { get; set; }
        public bool? Isprimarypublisher { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual SiteBooking Sitebooking { get; set; }
    }
}
