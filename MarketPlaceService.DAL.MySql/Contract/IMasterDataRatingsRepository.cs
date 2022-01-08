using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMasterDataRatingsRepository
    {
        Task<IEnumerable<MasterDataRating>> GetMasterDataRatings(int ratingType);
        Task<MasterDataRating> GetMasterDataRatings(int ratingType, int ratingId);
        Task<MasterDataRating> InsertMasterDataRatings(int ratingType,MasterDataRating data);
        Task<MasterDataRating> UpdateMasterDataRatings(int ratingType, int ratingId, MasterDataRating data);
        Task<bool> DeleteMasterDataRatings(int ratingType, int ratingId);
        Task<bool> CheckIfMappedToImportedProduct(int ratingId);
        Guid UserId { get; set; }
    }
}
