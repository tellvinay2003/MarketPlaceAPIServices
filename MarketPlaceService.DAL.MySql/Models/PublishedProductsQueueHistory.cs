using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublishedProductsQueueHistory
    {
        public Guid PublishedProductsQueueHistoryId { get; set; }
        public short PublishedProductTypeId { get; set; }
        public Guid PublishedProductId { get; set; }
        public Guid PublisherId { get; set; }
        public string ProcessingNote { get; set; }
        public int RetryCount { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime JobStartDateTime { get; set; }
        public DateTime JobEndDateTime { get; set; }
        public int? ProductId { get; set; }
        public string ProcessedBy { get; set; }
        public short? Productsubstatusid { get; set; }
        public short? Jobhistorystatusid { get; set; }
        public Guid? Traceid { get; set; }
        public Guid? Publishedproductqueueid { get; set; }
        public short? Jobtypeid { get; set; }

        public virtual JobHistoryStatus Jobhistorystatus { get; set; }
        public virtual JobType Jobtype { get; set; }
        public virtual PublisherProductSubStatus Productsubstatus { get; set; }
    }
}
