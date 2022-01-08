using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IPackageStatuses
    {
        Task<string> GetPackageStatusAsync(Guid entityId, EntityType entityType);

        Guid TraceId { get; set; }
        
    }
}
