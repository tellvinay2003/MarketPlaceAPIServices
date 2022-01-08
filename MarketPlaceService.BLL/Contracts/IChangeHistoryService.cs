
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IChangeHistoryService
    {
        Task<IEnumerable<ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site);
        Task<IEnumerable<ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site, int pagenumber);

        Guid TraceId { get; set; }
    }
}
