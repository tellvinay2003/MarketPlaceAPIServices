using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Contract
{
    public interface ITransactionLoggerRepository<T>
    {
        Task<int> SaveTransactionLog(TransactionLogDataModel<T> logItem);
    }
}
