using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.UtilityService;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using MarketPlaceService.Utilities;

namespace MarketPlaceService.BLL
{
    public class CommonService : ICommonService
    {
        private readonly ILogger<CommonService> _logger;
        private readonly ICommonRepository _commonRepository;
        private readonly ITravelStudioService _travelStudioService;
        private readonly IAPIManagerService _apiManagerService;

        public CommonService(ILogger<CommonService> logger, ICommonRepository commonRepository, ITravelStudioService travelStudioService, IAPIManagerService apiManagerService)
        {
            _logger = logger;
            _commonRepository = commonRepository;
            _travelStudioService = travelStudioService;
            _apiManagerService = apiManagerService;
        }

        public async Task<Tuple<bool, string>> CanOrganisationBeUsed(int organisationId, EntityType entityType, Guid entityId, Guid siteId)
        {
            var organisationsTask = _apiManagerService.GetResponseAsync(TravelStudioControllers.Organisations, "", null, null, EntityType.Site, siteId);
            var organisationIdsAlreadyUsedTask = _commonRepository.GetOrganisationIdsUsedForSite(entityType, entityId, siteId);

            var organisationIdsAlreadyUsed = await organisationIdsAlreadyUsedTask;
            var organisationsString = await organisationsTask;
            var organisations = JsonConvert.DeserializeObject<List<OrganizationDataModel>>(organisationsString);

            //if a root leve org has already been used then return false since u cannot add any new organisation for the site
            var hasParentOrgBeenUsed = HasParentOrganisationBeenUsed(organisationIdsAlreadyUsed, organisations);
            if (hasParentOrgBeenUsed.Item1)
                return new Tuple<bool, string>(false, $"This site already has a {entityType.ToString()} registered for the root level organisation: {hasParentOrgBeenUsed.Item2}. Registering another {entityType.ToString()} is not permitted.");
            
            //if the new organisation is a root or and any organisations have already been saved for the site then return false since we cannot save it.
            var newOrganisation = organisations.FirstOrDefault(a => a.Organisationid == organisationId);
            if (newOrganisation.ParentOrganisationid == null && organisationIdsAlreadyUsed.Any())
                return new Tuple<bool, string>(false, $"This site already has a {entityType.ToString()} registered with a level 1 organisation. Registering another {entityType.ToString()} with a root level organisation is not permitted.");

            return new Tuple<bool, string>(true,string.Empty);

        }

        private Tuple<bool, string> HasParentOrganisationBeenUsed(List<int> usedOrganisationIds, List<OrganizationDataModel> allOrganisations)
        {
            var parentOrgAlreadyUsed = usedOrganisationIds.Any(q => q == allOrganisations.FirstOrDefault(a => a.ParentOrganisationid == null).Organisationid);
            if(parentOrgAlreadyUsed)
            {
                return new Tuple<bool, string>(true, allOrganisations.FirstOrDefault(a => usedOrganisationIds.Contains((int)a.Organisationid) && a.ParentOrganisationid == null).Organisationname);
            }

            return new Tuple<bool, string>(false, string.Empty);
        }

        public async  Task<bool> DuplicateMasterDataNameExists(int dataTypeId, string dataTypeName, int dataId, string dataName, int ratingTypeId)
        {
            try
            {
                _logger.LogInformation("Repository call for GetChangeHistory started");
                var watch = Stopwatch.StartNew();
                var result =  _commonRepository.DuplicateMasterDataNameExists(dataTypeId, dataTypeName, dataId, dataName, ratingTypeId).Result;
                watch.Stop();
                _logger.LogInformation("Execution Time of GetChangeHistory repository call is: {duration}ms", watch.ElapsedMilliseconds);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public string GetJsonDifference(string originalJson, string changedJson, JsonType type)
        {
            JObject oldJson = JObject.Parse(originalJson);
            JObject newJson = JObject.Parse(changedJson);            

            var result = JsonUtility.GetDiffJson(oldJson, newJson, type);
            return result.Item2.ToString();
        }
    }
}
