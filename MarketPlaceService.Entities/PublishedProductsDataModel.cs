using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class PublishedProductsDataModel
    {
        public string PublisherName { get; set; }
        public string  ProductName { get; set; }

        public string  SupplierName { get; set; }

        public string  ServiceType { get; set; }

        public string LocationName { get; set; }

        public string Status { get; set; }

        public bool IsPublished { get; set; }

        // public Guid? PublishedProductId { get; set; }

        public Guid? PublishedProductQueueId { get; set; }


        public Guid PublishedProductId { get; set; }
        public Guid PublisherId { get; set; }
        public ProductType ProductTypeId { get; set; }
        public int ProductId { get; set; }
        public int ProductVersion { get; set; }
        public string ProductData { get; set; }
        public short? PublishedStatusId { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public short? PublisherProductStatusId { get; set; }
        public string ProcessingNote { get; set; }
        public string ProcessedBy { get; set; }
        public short? PublishSubStatus { get; set; }
        public Guid? PublishedBy { get; set; }
        public string ProcessingStatusName { get; set; }
        public bool IsPublishable { get; set; }
        public List<Error> Errors { get; set; }
        public short? ProductSubStatusId { get; set; }
        public string ProductSubStatusName { get; set; }
        public string ErrorMessage { get; set; }
        public Guid? TraceId {get;set;}
        public string ProductUpdateDifferenceData { get; set; }
        public short? JobTypeId { get; set; }
    }
}
