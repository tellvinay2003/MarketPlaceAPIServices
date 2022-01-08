using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlaceService.BLL
{
    public class ChangeHistoryService : IChangeHistoryService
    {
        private readonly ILogger<ChangeHistoryService> _logger;
        private readonly IChangeHistoryRepository _changeHistoryRepository;

        private Guid _traceId;
        public Guid TraceId
        {
            get
            {
                if (_traceId == Guid.Empty)
                    _traceId = Guid.NewGuid();
                return _traceId;
            }
            set
            {
                _traceId = value;
            }
        }

        public ChangeHistoryService(ILogger<ChangeHistoryService> logger, IChangeHistoryRepository changeHistoryRepository)
        {
            _logger = logger;
            _changeHistoryRepository = changeHistoryRepository;
        }
        public async Task<IEnumerable<ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetChangeHistory", "ChangeHistoryService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _changeHistoryRepository.GetChangeHistory(dataType, origin, site);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetChangeHistory", "ChangeHisotryRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetChangeHistory", "ChangeHistoryService", TraceId);
            return result;
        }

        public async Task<IEnumerable<ChangeHistory>> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site, int pagenumber)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetChangeHistory", "ChangeHistoryService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _changeHistoryRepository.GetChangeHistory(dataType, origin, site, pagenumber);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetChangeHistory", "ChangeHisotryRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetChangeHistory", "ChangeHistoryService", TraceId);
            return result;
        }
    }
}
