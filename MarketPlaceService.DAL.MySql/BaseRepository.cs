
using MarketPlaceService.DAL.Models;

namespace MarketPlaceService.DAL
{
    public abstract class BaseRepository
    {
        protected readonly MarketplaceDbContext _context;

        public BaseRepository(MarketplaceDbContext context)
        {
            _context = context;
        }
    }
}
