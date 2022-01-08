using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IAgentsService
    {
         Task<string> GetAgents(Guid entityId,EntityType entityType, string agentName, int limit);
         Task<string> GetAgentById(Guid entityId,EntityType entityType, int agentId);     
         Guid TraceId { get; set; }   
    }
}
