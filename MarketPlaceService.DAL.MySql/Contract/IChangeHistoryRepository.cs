using MarketPlaceService.DAL.Models;
using MarketPlaceService.DAL.Utilities;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Contract
{
    public interface IChangeHistoryRepository
    {
        Task<IEnumerable<Entities.ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site);
        Task<IEnumerable<Entities.ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site, int pagenumber);
        //Task<Entities.ChangeHistory> InsertChangeHistory(Entities.ChangeHistory request);

        Task SaveHistory(int dataType, HistoryAction action, HistoryOrigin origin, Guid who, object sourceObject, object targetObject, Guid site, IChangeHistoryHelper changeHistoryHelper);
    }
}
