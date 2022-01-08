using System;
using System.ComponentModel.DataAnnotations;

namespace MarketPlaceService.Entities
{
    public class DataMap
    {       
        [Required]
        [Range(0, UInt32.MaxValue)]
        public int TargetId { get; set; }
        public string SourceName { get; set; }
        public string TargetName { get; set; }

        public Guid MappingDataId {get;set;}
    }

    public class DataMapResponse : DataMap
    {
         public int SourceId { get; set; }
    }
}
