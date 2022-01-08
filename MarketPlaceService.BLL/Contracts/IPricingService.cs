using MarketPlaceService.Entities;
using MarketPlaceService.Entities.TSv2ApiEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IPricingService
    {
        Guid TraceId { get; set; }
        Task<GetServicePricesResponse> GetServicePricesFromTs(GetServicePricesRequest request);
        Task<GetServiceExtraPricesResponse> GetServiceExtraPrices(GetServiceExtraPricesRequest request);
        Task<Response<CalculateBookingPriceResponse>> GetBookingPrice(CalculateBookingPriceRequest request);
        Task<Guid> GetSubscriberId(Guid siteId, int organisationId);
    }
}
