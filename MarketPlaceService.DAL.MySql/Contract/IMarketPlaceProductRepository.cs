using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using MarketPlaceService.Entities;
using MarketPlaceService.DAL.MySql.Models;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMarketPlaceProductRepository
    {
       Task<string> GetMarketPlaceProduct(SubscriberProductDataRequest request);
    }
}
