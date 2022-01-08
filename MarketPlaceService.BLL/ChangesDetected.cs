using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketPlaceService.BLL
{
    public class ChangesDetectedService: IChangesDetectedService
    {
        public ChangesDetectedService()
        {

        }
        public List<ChangeDetected> GetChangesDetected(List<ChangeDetected> changesDetected, string propertyName)
        {
            if (changesDetected == null)
                return new List<ChangeDetected>();
            return changesDetected.Where(a => a.FieldName.Split('-').First().ToLower().Equals(propertyName)).ToList();
        }

        public bool HasobjectBeenUpdated(List<ChangeDetected> changesDetected)
        {
            if (changesDetected == null)
                return false;

            return changesDetected.Any(a => a.ChangeType.ToLower() == "changed");
        }
    }
}
