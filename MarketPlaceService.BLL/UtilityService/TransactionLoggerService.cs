using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.UtilityService
{
    public class TransactionLoggerService<T> : ITransactionLoggerService<T>
    {
        private readonly ITransactionLoggerRepository<T> _transactionLoggerRepository;

        private readonly ILogger<TransactionLoggerService<T>> _logger;

        public TransactionLoggerService(ITransactionLoggerRepository<T> transactionLoggerRepository, ILogger<TransactionLoggerService<T>> logger)
        {
            _transactionLoggerRepository = transactionLoggerRepository;
            _logger = logger;
        }
        public async Task<int> SaveTransactionLog(TransactionLogDataModel<T> logItem)
        {
           // _logger.LogInformation("Repository call for SaveTransactionLog started");
            var watch = Stopwatch.StartNew();
            var result = await _transactionLoggerRepository.SaveTransactionLog(logItem);
            watch.Stop();
           // _logger.LogInformation("Execution Time of SaveTransactionLog repository call is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }
    }
}
