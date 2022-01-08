using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL
{
    public class TransactionLoggerRepository<T> : BaseRepository, ITransactionLoggerRepository<T>, IHealthCheck
    {
        public TransactionLoggerRepository(MarketplaceDbContext context) : base(context)
        {

        }
        public async Task<int> SaveTransactionLog(TransactionLogDataModel<T> logItem)
        {
            TransactionLog transactionLog = new TransactionLog
            {
                TransactionData = JsonConvert.SerializeObject(logItem.TransactionData),
                TransactionStatus = logItem.TransactionStatus,
                TransactionType = logItem.TransactionType,
                TraceId = logItem.TraceId,
                InitiatedBy = logItem.InitiatedBy,
                InitiatedOn = logItem.InitiatedOn
            };

            _context.TransactionLog.Add(transactionLog);

           return await _context.SaveChangesAsync();
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
