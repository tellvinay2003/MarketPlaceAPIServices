using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class ApplyMappingRequest
    {
        public string JsonMessage { get; set; }

        public IEnumerable<ReplaceTagsResponse> tags { get; set; }
        public short? ProductTypeId { get; set; }
    }
}
