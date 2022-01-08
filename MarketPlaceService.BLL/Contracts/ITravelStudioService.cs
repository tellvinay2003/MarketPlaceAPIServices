using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Job;

namespace MarketPlaceService.BLL.Contracts
{
    public interface ITravelStudioService
    {
        Guid TraceId { get; set; }
        Task<string> GetAllBookingTypesAsync(Guid entityId,EntityType entityType);

        Task<string> GetAllPriceTypesAsync(Guid entityId,EntityType entityType);

        Task<string> GetAllCommunicationTypesAsync(Guid entityId,EntityType entityType);

        Task<string> GetAllSeasonTypesAsync(Guid entityId,EntityType entityType);

        Task<string> GetAllChargingPolicyAsync(Guid entityId,EntityType entityType);

        Task<string> GetSuppliersAsync(Guid entityId,EntityType entityType,string SupplierName);

        Task<string> GetSupplierNameAsync(Guid entityId,EntityType entityType,int supplierId);
        Task<string> GetProductCodesAsync(Guid entityId,EntityType entityType);

        Task<string> GetRegions(Guid entityId, EntityType entityType, string name);
        Task<string> GetServiceTypesAsync(Guid entityId, EntityType entityType);

        Task<string> GetRegionById(Guid entityId,EntityType entityType,int id);
        Task<string> GetOrganization(Guid entityId, EntityType entityType);

        Task<string> GetServiceTypeTypes(Guid entityId, EntityType entityType);

        Task<string> GetMappingTypesSiteData(Guid entityId, EntityType entityType, int dataTypeid, int? filterId);

        Task<string> GetAllTaxesAsync(Guid entityId, EntityType entityType);

        Task<string> GetOrganizationsPrefix(Guid entityId, EntityType entityType, int organisationId);

        Task<string> GetUsers(Guid entityId, EntityType entityType,string name,int limit);

        Task<string> GetUserById(Guid entityId, EntityType entityType,int userid);

        Task<string> GetBookingStatus(Guid entityId, EntityType entityType);

        Task<string> SearchJob(Guid entityId, EntityType entityType,BusinessProcess businessProcess, Enum queue , Enum jobType);

        Task<string> GetPackageTypesAsync(Guid entityId, EntityType entityType);


    }
}
