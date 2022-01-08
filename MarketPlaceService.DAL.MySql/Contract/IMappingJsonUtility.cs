using System;
using System.Threading.Tasks;
using MarketPlaceService.Entities;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMappingJsonUtility
    {
        IEnumerable<MessageFieldDetails> GetMessageFieldDetails(int MessageTypeID);
        IEnumerable<ReplaceTagsResponse> GetReplaceTags(GetReplaceTagsRequest request);
    }
}
