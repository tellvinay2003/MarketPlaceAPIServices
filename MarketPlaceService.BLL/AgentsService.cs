using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using CommonUtilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.UtilityService;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketPlaceService.BLL
{
    public class AgentsService : IAgentsService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IAPIManagerService _apiManagerService;
        private readonly ILogger<AgentsService> _logger;

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

        public AgentsService(ICommonRepository commonRepository, IAPIManagerService apiManagerService,ILogger<AgentsService> logger)
        {
            _commonRepository = commonRepository;
            _apiManagerService=apiManagerService;
            _logger = logger;
        }
        public async Task<string> GetAgentById(Guid entityId,EntityType entityType, int agentId)
        {
           //var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            //var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/Agents/{1}", url,agentId));
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAgentById", "AgentService", TraceId);
            var watch = Stopwatch.StartNew();
            List<APIParam> mandatoryParameters = new List<APIParam>
            {
                new APIParam{ Value = agentId.ToString()}
            } ;
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Agents,"",mandatoryParameters, null, entityType, entityId);
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAgentById", "AgentService", TraceId);
            return result;
        }

        public async Task<string> GetAgents(Guid entityId,EntityType entityType, string agentName, int limit)
        {
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await _apiManagerService.GetResponseAsync(string.Format("{0}api/v1/Agents/search?name={1}&limit={2}", url, agentName, limit));
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAgents", "AgentService", TraceId);
            var watch = Stopwatch.StartNew();
            var optionalParams = new List<APIParam>{
                new APIParam { Name= "name", Value =  HttpUtility.UrlEncode(agentName)},
                new APIParam { Name = "limit", Value = limit.ToString()}
            };            
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Agents,"search",null,optionalParams,entityType,entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAgents", "AgentService", TraceId);
            return result;
        }
    }
}
