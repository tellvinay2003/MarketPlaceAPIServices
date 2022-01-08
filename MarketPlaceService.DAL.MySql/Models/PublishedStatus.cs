using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublishedStatus
    {
        public PublishedStatus()
        {
            PublishedProducts = new HashSet<PublishedProducts>();
        }

        public short PublishedStatusId { get; set; }
        public string PublishedStatusName { get; set; }

        public virtual ICollection<PublishedProducts> PublishedProducts { get; set; }
    }
}
