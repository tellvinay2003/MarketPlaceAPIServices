using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System.Linq;
using MarketPlaceService.DAL.Utilities;

namespace MarketPlaceService.DAL
{
    public class MarketPlaceProductRepository :BaseRepository, IMarketPlaceProductRepository
    {
        private readonly ICommonRepository _commonRepository;
        public MarketPlaceProductRepository(MarketplaceDbContext context,ICommonRepository commonRepository) : base(context)
        {
            _commonRepository = commonRepository;
        }

        public async Task<string> GetMarketPlaceProduct(SubscriberProductDataRequest request)
        {            
            var MarKetPlaceProductDetails=_context.MarketplaceProduct.Where(mp=>mp.Marketplaceproductid==request.MarketPlaceProductId).Select(a=>a.Productdata).FirstOrDefault();
            return await Task.FromResult(MarKetPlaceProductDetails);
        }
    }
}
