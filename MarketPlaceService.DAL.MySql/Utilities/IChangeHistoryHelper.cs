using System;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Utilities
{
    public interface IChangeHistoryHelper
    {
        // Task<string> GetStringForAdd(object source);
        // Task<string> GetStringForUpdate(object source, object target);
        // Task<string> GetStringFoDelete(object source);
        Task<string> GetDetails(HistoryAction action, object source, object target);
    }
}
