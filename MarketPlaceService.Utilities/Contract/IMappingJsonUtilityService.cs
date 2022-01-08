using System;
using MarketPlaceService.Entities;
using System.Collections.Generic;

namespace MarketPlaceService.Utilities
{
    public interface IMappingJsonUtilityService
    {
        ProcessJSONResponse ProcessJsonMapping(ProcessJSONRequest request);
        IEnumerable<RetriveFieldPathResponse> RetrieveValue (IEnumerable<string> fieldPath, string JsonMessage) ;
    }
}
