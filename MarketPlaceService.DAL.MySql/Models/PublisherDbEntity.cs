using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketPlaceService.DAL.MySql.Models
{
   public class PublisherDbEntity
    {
        public Guid PublisherId { get; set; }

        [Required]
        public string PublisherName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public string Description { get; set; }
        
        [Required]
        public Guid LocationId { get; set; }

        [Required]
        public Guid SupplierId { get; set; }
        public Guid SubscriberId { get; set; }

        [Required]
        public Guid ParentOrganisationId { get; set; }

        [Required]
        public Guid OrganisationTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string ShortName { get; set; }

        [Required]
        public Guid StatusId { get; set; }
        public Guid CancellationPolicyId { get; set; }
        public Guid CostGLAAccountId { get; set; }
        public Guid RevenueGLAccountId { get; set; }
    }
}
