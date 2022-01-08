using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IChangesDetectedService
    {
        List<ChangeDetected> GetChangesDetected(List<ChangeDetected> changesDetected, string propertyName);
        bool HasobjectBeenUpdated(List<ChangeDetected> changesDetected);
    }
}
