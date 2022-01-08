using System;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IServiceStatusesService
    {
        Task<string> GetServiceStatusAsync(Guid entityId, EntityType entityType);

        Guid TraceId { get; set; }
    }
}
