using System;
using System.Threading.Tasks;
using MarketPlaceService.Entities;
using MarketPlaceService.DAL.Models;

namespace MarketPlaceService.DAL.Utilities
{
    public class MasterDataChangeHistoryHelper : IChangeHistoryHelper
    {
        public async Task<string> GetDetails(HistoryAction action, object source, object target)
        {
             switch(action)
            {
                case Entities.HistoryAction.Add:
                return await GetStringForAdd(source);
                case Entities.HistoryAction.Delete:
                return await GetStringFoDelete(source);
                case Entities.HistoryAction.Change:
                return await GetStringForUpdate(source, target);
            }
            return string.Empty;
        }

        private async Task<string> GetStringFoDelete(object source)
        {
            var data = source as Models.MasterData;
            return $"Removed {data.Masterdataname}";
        }

        private async Task<string> GetStringForAdd(object source)
        {
            var data = source as Models.MasterData;
            return $"Added {data.Masterdataname}";
        }

        private async Task<string> GetStringForUpdate(object source, object target)
        {
            var sourceData = source as Models.MasterData;
            var targetData = target as Entities.MasterData;

            return $"{sourceData.Masterdataname} changed to {targetData.Name}";
        }
    }
}
