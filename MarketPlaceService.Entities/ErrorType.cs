using System;

namespace MarketPlaceService.Entities
{
    public enum ErrorType
    {
        SaveData,
        DataMappingIntegrity,
        ReadingMapping,
        FetchingProductData,        
        Unknown,
        SubscriptionPost
    }
}
