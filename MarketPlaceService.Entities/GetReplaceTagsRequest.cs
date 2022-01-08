using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class GetReplaceTagsRequest
    {
        public Guid siteId {get; set;}
        public int mappingDirection { get; set;}
        public IEnumerable<MessageFieldDetails> messageFields { get; set; }
    }
}
