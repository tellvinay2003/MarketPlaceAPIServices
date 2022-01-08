using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MarketPlaceService.DAL.Models;


namespace MarketPlaceService.DAL
{
    public class SupplierStatusesRepository : BaseRepository, ISupplierStatusesRepository
    {
        public SupplierStatusesRepository(MarketplaceDbContext context) : base(context)
        {

        }
        
    }
}
