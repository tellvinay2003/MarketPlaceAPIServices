using System;
using System.Threading.Tasks;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.UtilityService;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using System.Linq;
using System.Collections.Generic;

namespace MarketPlaceService.BLL
{
    public interface IAPIManagerHelperService
    {
        string GetUrl(TravelStudioControllers controller, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId);
        string GetUrl(TravelStudioControllers controller, string url);
    }

    public class APIManagerHelperService : IAPIManagerHelperService
    {
        private readonly ICommonRepository _commonRepository;

        public APIManagerHelperService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
        public string GetUrl(TravelStudioControllers controller, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId)
        {
            var url = _commonRepository.GetSiteUrl(entityId, entityType).Result;
            
            if(string.IsNullOrEmpty(url))
                return string.Empty;
            
            url = url + GetUrlExtention(controller);

            if (!string.IsNullOrEmpty(additionalRoute))
                url = string.Format("{0}/{1}", url, additionalRoute);

            if (routeParameters != null && routeParameters.Count > 0)
            {
                url = string.Format("{0}/{1}", url, string.Join("/", routeParameters.Select(q => q.Value).ToArray()));
            }

            if (optionalParameters != null && optionalParameters.Count > 0)
            {
                url = string.Format("{0}?{1}", url, string.Join("&", optionalParameters.Select(q => string.Format("{0}={1}", q.Name, q.Value)).ToArray()));
            }

            return url;
        }

        public string GetUrl(TravelStudioControllers controller, string url)
        {                
            return  url + GetUrlExtention(controller);
        }

        private string GetUrlExtention(TravelStudioControllers tsControllers)
        {
            string controllerName = "";
            var version = GetAPIVersion();
            var urlPartial = string.Format("api/{0}/",version);
            switch (tsControllers)
            {
                case TravelStudioControllers.Agents:
                    controllerName = "agents";
                    break;
                case TravelStudioControllers.BookingTypes:
                    controllerName = "bookingtypes";
                    break;
                case TravelStudioControllers.ChargingPolicies:
                    controllerName = "chargingpolicies";
                    break;
                case TravelStudioControllers.CommunicationTypes:
                    controllerName = "communicationtypes";
                    break;
                case TravelStudioControllers.Organisations:
                    controllerName = "organisations";
                    break;
                case TravelStudioControllers.PriceTypes:
                    controllerName = "pricetypes";
                    break;
                case TravelStudioControllers.ProductCodes:
                    controllerName = "productcodes";
                    break;
                case TravelStudioControllers.Products:
                    controllerName = "products";
                    break;
                case TravelStudioControllers.PublishedProducts:
                    controllerName = "controller";
                    break;
                case TravelStudioControllers.Regions:
                    controllerName = "regions";
                    break;
                case TravelStudioControllers.SeasonTypes:
                    controllerName = "seasontypes";
                    break;
                case TravelStudioControllers.ServiceStatuses:
                    controllerName = "servicestatuses";
                    break;
                case TravelStudioControllers.Sites:
                    controllerName = "sites";
                    break;
                case TravelStudioControllers.Suppliers:
                    controllerName = "suppliers";
                    break;
                case TravelStudioControllers.SupplierStatuses:
                    controllerName = "supplierstatuses";
                    break;
                case TravelStudioControllers.ServiceTypes:
                    controllerName = "servicetypes";
                    break;
                case TravelStudioControllers.MappingDataTypes:
                    controllerName = "mappingdatatypes";
                    break;
                case TravelStudioControllers.QueuedSubscribedProducts:
                    controllerName = "queuedSubscribedProducts";
                    break;
                case TravelStudioControllers.Taxes:
                   controllerName = "Taxes";
                   break;
                case TravelStudioControllers.Pricing:
                    controllerName = "Pricing";
                    break;
                case TravelStudioControllers.Booking:
                    controllerName = "Booking";
                    break;
                case TravelStudioControllers.Users:
                    controllerName = "Users";
                    break;
                case TravelStudioControllers.Job:
                    controllerName = "Job";
                    break;  
                case TravelStudioControllers.PackageTypes:
                    controllerName = "PackageTypes";
                    break;
                case TravelStudioControllers.PackageStatuses:
                    controllerName = "packageStatuses";
                    break;  
            }
            
            return urlPartial + controllerName;
        }

        private string GetAPIVersion()
        {
           return "v1";
        }
    }
}
