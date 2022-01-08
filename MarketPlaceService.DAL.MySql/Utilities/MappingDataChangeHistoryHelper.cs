using System;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Utilities
{
    public class MappingDataChangeHistoryHelper : IChangeHistoryHelper
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
            var data = source as Models.MappingData;
            return $"Removed {data.Sourcename}";
        }

        private async Task<string> GetStringForAdd(object source)
        {
            var data = source as Models.MappingData;
            return $"Mapped {data.Sourcename} to {data.Targetname}.";
        }

        private async Task<string> GetStringForUpdate(object source, object target)
        {
            var sourceData = source as Models.MappingData;
            var targetData = target as Entities.DataMapResponse;

            return $"{sourceData.Sourcename} mapped to {targetData.TargetName} from {sourceData.Targetname}";
        }
    }
}
