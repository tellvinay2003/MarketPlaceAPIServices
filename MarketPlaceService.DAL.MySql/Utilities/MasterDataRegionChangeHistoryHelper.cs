using System;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Utilities
{
    public class MasterDataRegionChangeHistoryHelper : IChangeHistoryHelper
    {
        public async Task<string> GetDetails(Entities.HistoryAction action, object source, object target)
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
            var data = source as Models.MasterRegions;
            return $"Removed {data.Regionname} from Level {data.Level}";
        }

        private async Task<string> GetStringForAdd(object source)
        {
            var data = source as Models.MasterRegions;
            return $"Added {data.Regionname} to Level {data.Level}";
        }

        private async Task<string> GetStringForUpdate(object source, object target)
        {
            var sourceData = source as Models.MasterRegions;
            var targetData = target as Entities.MasterDataGeolocation;

            return $"{sourceData.Regionname} changed to {targetData.Name} at Level {sourceData.Level}";
        }
    }
}
