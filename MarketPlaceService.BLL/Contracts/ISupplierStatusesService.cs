using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface ISupplierStatusesService
    {
        Task<string> GetAllSupplierStatusesAsync(Guid entityId,EntityType entityType);
        Task<string> GetSupplierStatusByIdAsync(Guid entityId,EntityType entityType, int supplierStatusId);
        Guid TraceId { get; set; }
        
    }
}
