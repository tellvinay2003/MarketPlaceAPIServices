using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts.UtilityServiceContracts
{
    public interface ITransactionLoggerService<T>
    {
        Task<int> SaveTransactionLog(TransactionLogDataModel<T> logItem);
    }
}
