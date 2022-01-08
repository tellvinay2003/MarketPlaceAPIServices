using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Polly;
//using MarketPlaceService.BLL;
//using MarketPlaceService.BLL.Contracts;
//using MarketPlaceService.BLL.Models;
using MarketPlaceService.DAL;
using MarketPlaceService.DAL.Contract;
using System;
using System.Linq;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System.Collections.Generic;

namespace MarketPlaceService.Utilities
{
    public class MappingJsonUtilityService : IMappingJsonUtilityService
    {
        private readonly IMappingJsonUtility _mappingJsonUtility;
        public MappingJsonUtilityService(IMappingJsonUtility mappingJsonUtility)
        {
            _mappingJsonUtility = mappingJsonUtility;
        }

        public ProcessJSONResponse ProcessJsonMapping(ProcessJSONRequest request)
        {
            var response = new ProcessJSONResponse();
            response.IsSuccess = true;
            try
            {
                //Get list of message fields
                var messageDetails = _mappingJsonUtility.GetMessageFieldDetails(request.MessageTypeId);

                GetReplaceTagsRequest req = new GetReplaceTagsRequest();
                req.siteId = request.SiteId;
                req.mappingDirection = request.MappingDirection;
                req.messageFields = messageDetails;
                //get source and target ids
                var replaceTagss = _mappingJsonUtility.GetReplaceTags(req);
                //Now call the ApplyMapping method by passing the JSON message and the list of messageFields

                var applyMappingRequest = new ApplyMappingRequest();
                applyMappingRequest.tags = replaceTagss;
                applyMappingRequest.JsonMessage = request.JsonString;
                applyMappingRequest.ProductTypeId = request.ProductTypeId;
                //applyMappingRequest.tags = messageDetails;
                var applyMappingResponse = ApplyMapping(applyMappingRequest, false, null);
                response.JsonString = applyMappingResponse.JsonString;
                response.MappingError = applyMappingResponse.MappingError;
                response.Description = applyMappingResponse.Description;
                if(response.Description.Count() > 0)
                {
                    response.IsSuccess = false;
                    response.JsonString  = null;
                }

              if(request.subscriberDefaults != null)
              {
                    //JObject objToMap = JObject.Parse(response.JsonString);
                  //var subscriberDefaults = _mappingJsonUtility.GetSubscriberDefaults(request.SubscriberId, request.MarketplaceProductId);
                  
                    var mappingErrors = ValidateMandatoryDefaultsMapping(request.subscriberDefaults, applyMappingResponse.JsonString, request.AllRegions);
                    if(mappingErrors.Count() > 0)
                    {
                         response.Description.AddRange(mappingErrors);
                         response.IsSuccess = false;
                        response.JsonString  = null;
                    }
                   

                    //if all mandatory defaults exists create apply mapping request
                     if(response.Description.Count() == 0)
                     {
                        ApplyMappingRequest defaultMappingRequest = new ApplyMappingRequest();
                        defaultMappingRequest.JsonMessage = response.JsonString;
                        defaultMappingRequest.tags = GetSubscriberDefaultTags(request.subscriberDefaults);
                        defaultMappingRequest.ProductTypeId = request.ProductTypeId;

                        applyMappingResponse = ApplyMapping(defaultMappingRequest, true, request.subscriberDefaults.ChargingPolicy.DefaultChargingPolicy);
                        response.JsonString = applyMappingResponse.JsonString;
                     }

              }

             
                 return response;
            }
            catch (Exception ex)
            {
                response.MappingError = MappingErrorType.Exception;
                response.Description  = null;
                response.Description.Add(ex.ToString());
                response.IsSuccess = false;
                  response.JsonString  = null;
                return response;
            }

        }
        private ProcessJSONResponse ApplyMapping(ApplyMappingRequest request, bool subscriptionProcess, List<ServiceTypeTypeChargingPolicy> chargingPolicies)
        {
            ProcessJSONResponse response = new ProcessJSONResponse();
            response.Description = new List<string>();
            JObject objToMap = JObject.Parse(request.JsonMessage);
            char[] separator = { '|' };

            foreach (ReplaceTagsResponse tag in request.tags)
            {
                if(request.ProductTypeId == 2 && (tag.DataTypeId == 3 || tag.DataTypeId == 4)) //dont check for option/extra status if its of type package
                {
                    continue;
                }
                if(tag.DataTypeName == "Extra Status" || tag.DataTypeName == "Itinerary Type")
                {
                 var exists = checkIfExists(tag.DataTypeName, request.JsonMessage, tag.TagFullPath);
                 tag.IsMappingMandatory = exists ? tag.IsMappingMandatory : false;
                }

                string[] splittedPath = tag.TagFullPath.Split(separator);
                int pathCount = splittedPath.Length;
                bool innerMapFound = false;
                bool nameMapped = false;
                bool mapNotFound = false;
                List<string> idToMapLst = new List<string>();
                var idToMap = string.Empty;
                var nameToMap = string.Empty;
                List<string> searchPathList = new List<string>();

                string pathToProcess = string.Empty;
                List<string> proccessedPath = new List<string>();
                int childCount = 0;

                if (pathCount > 1)
                {
                    //loop over splittedPath array
                    for (int i = 0; i < pathCount - 1; i++ ) //pathCount - 1: because last item is to be searched for
                    {
                        // check if it contains []
                        if (splittedPath[i].EndsWith("[]"))
                        {
                            splittedPath[i] = splittedPath[i].TrimEnd('[', ']');
                            if(proccessedPath.Count() > 0) //this is if array is present inside an array object 
                            {
                                List<string> toBeRemoved = new List<string>();
                                var parentCount = proccessedPath.Count();
                                for(int l = 0; l < parentCount; l++)
                                {
                                    var temp = string.Empty;
                                    
                                    childCount = objToMap.SelectTokens((proccessedPath[l] + "." + splittedPath[i]).ToString()).Children().Count();
                                    for (int j = 0; j < childCount; j++)
                                    {                                         
                                        temp = proccessedPath[l];
                                        toBeRemoved.Add(temp);
                                        
                                        proccessedPath.Add(temp + "." + splittedPath[i] + '[' + j.ToString() + ']');             
                                    }
                                }
                                //delete parentItems
                                for(int k = 0; k < toBeRemoved.Count(); k ++)
                                {
                                    proccessedPath.Remove(toBeRemoved[k]);
                                }
                            }
                            else
                            {   //first level array
                                pathToProcess = string.IsNullOrEmpty(pathToProcess) ? pathToProcess + splittedPath[i] : pathToProcess + '.' + splittedPath[i];
                                childCount = objToMap.SelectTokens(pathToProcess.ToString()).Children().Count();
                                for (int j = 0; j < childCount; j++)
                                {                                
                                    proccessedPath.Add(pathToProcess.ToString() + '[' + j.ToString() + ']');                            
                                }
                            }
                        }
                        else
                        {
                            for(int k = 0; k < proccessedPath.Count(); k++)  //append to all the items in the array
                            {
                                proccessedPath[k] = proccessedPath[k] + "." + splittedPath[i];
                            }
                            pathToProcess = pathToProcess + splittedPath[i];
                            if(proccessedPath.Count == 0)
                                proccessedPath.Add(pathToProcess);
                        }
                    }    
                    searchPathList = proccessedPath;                                 
                }
                else if (pathCount == 1)
                {
                    searchPathList.Add(""); //to be searched under root element
                }
                string[] idNameLst = null;
                //convert the tag values
               
               int count = searchPathList.Count();
                for (var t = count-1; t >= 0; t--)
                {
                    var searchPath = searchPathList[t];
                    innerMapFound = false;
                    nameMapped = false;
                    mapNotFound = false;
                    idNameLst = null;
                    idToMap = string.Empty;
                    idNameLst = splittedPath[pathCount - 1].ToString().Split(',');

                    foreach (var prop in objToMap.SelectTokens(searchPath.ToString()).Children().OfType<JProperty>())
                    {
                        if (prop.Name == idNameLst[0])
                        {
                            idToMap = prop.Value.ToString();
                           if( idNameLst.Count()>1)
                           {
                               if(!string.IsNullOrEmpty(searchPath))
                               {
                                   var nameProp = objToMap.SelectTokens(searchPath.ToString()).Children().OfType<JProperty>().Where(e=>e.Name == idNameLst[1]).FirstOrDefault();
                                   if(nameProp !=null && nameProp.Value !=null)   
                                       nameToMap =nameProp.Value.ToString();

                               }
                               else
                               {
                                   var nameProp = objToMap.Children().OfType<JProperty>().Where(e=>e.Name == idNameLst[1]).FirstOrDefault();
                                   if(nameProp !=null && nameProp.Value !=null)   
                                       nameToMap =nameProp.Value.ToString();
                               }                                
                           
                           }
                            foreach (var tagValue in tag.MappingValues)
                            {
                                if(tag.DataTypeName == "Charging Policy")
                                {
                                    //fetch type ID
                                    int index = searchPath.LastIndexOf('.');  
                                    var typeIdPath = searchPath.Substring(0,index);
                                    typeIdPath = typeIdPath + ".typeId";
                                    int typeId = (int)objToMap.SelectToken(typeIdPath);
                                    tagValue.TagetValueId = GetChargingPolicyId(typeId, chargingPolicies, tag.TagName);
                                }
                                
                                if (!innerMapFound && (prop.Value.ToString() == tagValue.SourceValueId.ToString() || (subscriptionProcess && tagValue.TagetValueId != null)))
                                {
                                    prop.Value = tagValue.TagetValueId;
                                    innerMapFound = true;
                                } 

                            }                               
                        }
                        if (idNameLst.Length > 1 && prop.Name == idNameLst[1])
                        {
                            foreach (var tagValue in tag.MappingValues)
                            {
                                if (!nameMapped &&(prop.Value.ToString() == tagValue.SourceValueName.ToString() || (subscriptionProcess && tagValue.TagetValueName != null)))
                                {
                                    prop.Value = tagValue.TagetValueName.ToString();
                                    nameMapped = true;
                                }
                                    
                            }
                        }
                       
                    }
                    if(!innerMapFound)
                    {
                        mapNotFound = true;
                        if (!string.IsNullOrEmpty(nameToMap.Trim()))
                            idToMapLst.Add(nameToMap.Trim()+"(Id: "+idToMap.ToString()+")");
                        else
                            idToMapLst.Add(idToMap.ToString());

                    }
                   
                    if(mapNotFound && tag.RemoveTag != null)
                    {
                        string[] deletePaths = searchPath.Split('.');
                        if(deletePaths.Length == 1)
                        {
                            if(deletePaths[0].EndsWith(']'))
                            {
                                //for(int j=0; j < idNameLst.Length; j++)
                            //{
                                JToken deleteToken = objToMap.SelectToken(searchPath+'.'+idNameLst[0]);
                                if(deleteToken != null)
                                {
                                    deleteToken.Parent.Parent.Remove();
                                } 
                                    
                            //}
                            }
                            else
                            {
                                objToMap.Remove(deletePaths[0]);
                            }
                            
                        }
                        else
                        {
                            if(tag.DataTypeId == 6)
                            {
                                JToken deleteToken = objToMap.SelectToken(searchPath+'.'+idNameLst[0]);
                                if(deleteToken != null)
                                {
                                    deleteToken.Parent.Parent.Remove();
                                } 
                            }
                            else
                            {
                            for(int j=0; j < idNameLst.Length; j++)
                            {
                                JToken deleteToken = objToMap.SelectToken(searchPath+'.'+idNameLst[j]);
                                if(deleteToken != null)
                                    deleteToken.Parent.Remove();
                            }                     
                            }
                        }
                    }
                }
                if(tag.IsMappingMandatory && mapNotFound && !subscriptionProcess)
                {
                    var strIds = idToMapLst.Distinct().Aggregate((i, j) => i + ", " + j).ToString();
                    response.MappingError = MappingErrorType.Integrity;
                    response.Description.Add(string.Concat(" Mandatory mapping not found for ",tag.DataTypeName.ToString(), " - ", strIds));
                }
            }
           // if(response.Description.Count() == 0)
            //{
                string jsonText = JsonConvert.SerializeObject(objToMap);
                response.JsonString = jsonText;
           // }            
            return response;
            }
       

        public IEnumerable<RetriveFieldPathResponse> RetrieveValue(IEnumerable<string> fieldPath, string JsonMessage)
        {
            JObject objToMap = JObject.Parse(JsonMessage);
            char[] separator = { '|' };
            List<RetriveFieldPathResponse> response = new List<RetriveFieldPathResponse>();

            foreach (var path in fieldPath)
            {
                RetriveFieldPathResponse tempObj = new RetriveFieldPathResponse();
                tempObj.Value = new List<string>();

                string[] splittedPath = path.Split(separator);
                int pathCount = splittedPath.Length;
                List<string> searchPathList = new List<string>();

                string pathToProcess = string.Empty;
                List<string> proccessedPath = new List<string>();
                int childCount = 0;

                if (pathCount > 1)
                {
                    //loop over splittedPath array
                    for (int i = 0; i < pathCount - 1; i++ ) //pathCount - 1: because last item is to be searched for
                    {
                        // check if it contains []
                        if (splittedPath[i].EndsWith("[]"))
                        {
                            splittedPath[i] = splittedPath[i].TrimEnd('[', ']');
                            if(proccessedPath.Count() > 0) //this is if array is present inside an array object 
                            {
                                List<string> toBeRemoved = new List<string>();
                                var parentCount = proccessedPath.Count();
                                for(int l = 0; l < parentCount; l++)
                                {
                                    var temp = string.Empty;
                                    
                                    childCount = objToMap.SelectTokens((proccessedPath[l] + "." + splittedPath[i]).ToString()).Children().Count();
                                    for (int j = 0; j < childCount; j++)
                                    {                                         
                                        temp = proccessedPath[l];
                                        toBeRemoved.Add(temp);
                                        
                                        proccessedPath.Add(temp + "." + splittedPath[i] + '[' + j.ToString() + ']');             
                                    }
                                }
                                //delete parentItems
                                for(int k = 0; k < toBeRemoved.Count(); k ++)
                                {
                                    proccessedPath.Remove(toBeRemoved[k]);
                                }
                            }
                            else
                            {   //first level array
                                pathToProcess = string.IsNullOrEmpty(pathToProcess) ? pathToProcess + splittedPath[i] : pathToProcess + '.' + splittedPath[i];
                                childCount = objToMap.SelectTokens(pathToProcess.ToString()).Children().Count();
                                for (int j = 0; j < childCount; j++)
                                {                                
                                    proccessedPath.Add(pathToProcess.ToString() + '[' + j.ToString() + ']');                            
                                }
                            }
                        }
                        else
                        {
                            for(int k = 0; k < proccessedPath.Count(); k++)  //append to all the items in the array
                            {
                                proccessedPath[k] = proccessedPath[k] + "." + splittedPath[i];
                            }
                            pathToProcess = pathToProcess + splittedPath[i];
                        }
                    }    
                    searchPathList = proccessedPath;                                 
                }
                else if (pathCount == 1)
                {
                    searchPathList.Add(""); //to be searched under root element
                }

                foreach (var searchPath in searchPathList)
                {
                  foreach (JProperty prop in objToMap.SelectTokens(searchPath.ToString()).Children().OfType<JProperty>())
                    {
                        if (prop.Name == splittedPath[pathCount - 1])
                        {
                            tempObj.Value.Add(prop.Value.ToString());
                        }
                    }
                }
                tempObj.FieldPath = path;
                response.Add(tempObj);
            }

            return response;
        }

            public IEnumerable<ReplaceTagsResponse> GetSubscriberDefaultTags(SubscriberDefaultsModelForSubscription subscriberDefaults)
            {
                List<ReplaceTagsResponse> response = new List<ReplaceTagsResponse>();
                ReplaceTagsResponse singleTag = new ReplaceTagsResponse();

                //Charging Policies Options
                singleTag.IsMappingMandatory = true;
                singleTag.TagFullPath = "options[]|type|chargingPolicy|id";
                singleTag.TagName = "optionId";
                singleTag.DataTypeName = "Charging Policy";
                singleTag.MappingValues = new List<MappingList>();
                 
                MappingList temp = new MappingList();
                temp.TagetValueId = null;
                   singleTag.MappingValues.Add(temp);
                
                response.Add(singleTag);

                 //Charging Policies Extras
                singleTag = new ReplaceTagsResponse();
                singleTag.IsMappingMandatory = true;
                singleTag.TagFullPath = "extras[]|type|chargingPolicy|id";
                singleTag.TagName = "extraId";
                singleTag.DataTypeName = "Charging Policy";
                singleTag.MappingValues = new List<MappingList>();
               
                temp = new MappingList();
                temp.TagetValueId = null;
                   singleTag.MappingValues.Add(temp);
                
                response.Add(singleTag);

                singleTag = new ReplaceTagsResponse();
                //Communication Type
                singleTag.IsMappingMandatory = true;
                singleTag.TagFullPath = "communicationTypeId";
                singleTag.TagName = "communicationTypeId";
                singleTag.DataTypeName = "Communication Type Id";
                singleTag.MappingValues = new List<MappingList>();
             
                temp = new MappingList();
                temp.TagetValueId = subscriberDefaults.ChargingPolicy.DefaultCommunicationType.Id;
                   singleTag.MappingValues.Add(temp);
                response.Add(singleTag);

                singleTag = new ReplaceTagsResponse();
                //Supplier
                singleTag.IsMappingMandatory = true;
                singleTag.TagFullPath = "supplier|id";
                singleTag.TagName = "id";
                singleTag.MappingValues = new List<MappingList>();
                singleTag.DataTypeName = "Supplier";
                singleTag.MappingValues= (from s in subscriberDefaults.Suppliers
                select new MappingList{
                    TagetValueId = s.SupplierId
                }).ToList();
                response.Add(singleTag);

                singleTag = new ReplaceTagsResponse();
                //Supplier
                singleTag.IsMappingMandatory = false;
                singleTag.TagFullPath = "options[]|productCode|id";
                singleTag.TagName = "id";
                singleTag.DataTypeName = "Option Product Code";
                singleTag.MappingValues = new List<MappingList>();
                singleTag.MappingValues= (from pc in subscriberDefaults.ProductCodeRules
                where pc.ApplyToOptions == true
                select new MappingList{
                    TagetValueId = pc.ProductCodeId
                }).ToList();
                response.Add(singleTag);

                singleTag = new ReplaceTagsResponse();
                //Supplier
                singleTag.IsMappingMandatory = false;
                singleTag.TagFullPath = "extras[]|productCode|id";
                singleTag.TagName = "id";
                singleTag.DataTypeName = "Extra Product Code";
                singleTag.MappingValues = new List<MappingList>();
                singleTag.MappingValues= (from pc in subscriberDefaults.ProductCodeRules
                where pc.ApplyToExtras == true
                select new MappingList{
                    TagetValueId = pc.ProductCodeId
                }).ToList();
                response.Add(singleTag);

                return response;
            } 

            public List<string> ValidateMandatoryDefaultsMapping(SubscriberDefaultsModelForSubscription subscriberDefaults,string jsonStr, List<RegionData> allRegions)
            {
                //check if mandatory mappings are present
                //Mandatory mappings are
                //Communication Type, Supplier, Season Type, Price Type, Booking Type, Charging Policy
                List<string> mappingErrors = new List<string>();
                var req = new List<string>();
                req.Add("options[]|type|typeId");
                req.Add("extras[]|type|typeId");
                req.Add("serviceTypeId");
                req.Add("regionId");
                var typeIds = RetrieveValue(req, jsonStr);
                int _regionId = Convert.ToInt32(typeIds.ToList()[3].Value.FirstOrDefault());
                int _serviceTypeId = Convert.ToInt32(typeIds.ToList()[2].Value.FirstOrDefault());
                var optionsExists = typeIds.ToList()[0].Value.Count() > 0 ? true : false;
                var extrasExists = typeIds.ToList()[1].Value.Count() > 0 ? true : false;
                var optionChargingPolicyExists = subscriberDefaults.ChargingPolicy.DefaultChargingPolicy.Any(a=> typeIds.ToList()[0].Value.Contains(a.ServiceTypeTypeId.ToString()) && a.OptionChargingPolicyId != null);
                var extraChargingPolicyExists = subscriberDefaults.ChargingPolicy.DefaultChargingPolicy.Any(a=> typeIds.ToList()[1].Value.Contains(a.ServiceTypeTypeId.ToString()) && a.ExtraChargingPolicyId != null);
             

                if((optionsExists && !optionChargingPolicyExists) || (extrasExists && !extraChargingPolicyExists))
                    mappingErrors.Add(string.Concat("Mandatory Default missing for ", "Charging Policy"));
                if(subscriberDefaults.ChargingPolicy.DefaultCommunicationType.Id == null)
                    mappingErrors.Add(string.Concat("Mandatory Default missing for ", "Communication Type"));
                if(subscriberDefaults.Defaults == null || (subscriberDefaults.Defaults != null && subscriberDefaults.Defaults.BuyBookingTypeID == null))
                    mappingErrors.Add(string.Concat("Mandatory Default missing for ", "Buy Booking Type"));
                if(subscriberDefaults.Defaults == null || (subscriberDefaults.Defaults != null && subscriberDefaults.Defaults.BuyPriceTypeID == null))
                    mappingErrors.Add(string.Concat("Mandatory Default missing for ", "Buy Price Type"));
                if(subscriberDefaults.Defaults == null || (subscriberDefaults.Defaults != null && subscriberDefaults.Defaults.SeasonTypeID == null))
                    mappingErrors.Add(string.Concat("Mandatory Default missing for ", "Season Type"));
                if(subscriberDefaults.Suppliers.Count() == 0)
                    mappingErrors.Add(string.Concat("Mandatory Default missing for ", "Supplier"));


                var sellingPricesSet = subscriberDefaults.SellingPrices.Where(a => (a.RegionId == null || GetRegionAndChildRegions((int)a.RegionId, allRegions).Contains(_regionId)) && (a.AllServiceTypesSelected==true || a.ServiceTypes.Contains(_serviceTypeId))).FirstOrDefault();
                if(sellingPricesSet == null)
                    mappingErrors.Add(string.Concat("Mandatory Default missing for ", "Selling Prices"));
              

                return mappingErrors;
            }

            public int GetChargingPolicyId(int typeId, List<ServiceTypeTypeChargingPolicy> chargingPolicies, string tagName)
            {
                var response = 0;
                
                if(tagName == "optionId")
                {
                   response =Convert.ToInt32(chargingPolicies.Where(a => a.ServiceTypeTypeId == typeId && a.OptionChargingPolicyId != null).Select(x=>x.OptionChargingPolicyId).FirstOrDefault());

                }
                else
                {
                    response =Convert.ToInt32(chargingPolicies.Where(a => a.ServiceTypeTypeId == typeId && a.ExtraChargingPolicyId != null).Select(x=>x.ExtraChargingPolicyId).FirstOrDefault());
                }


                return response;
            }

            public bool checkIfExists(string dataTypeName, string jsonString, string tagPath)
            {
                var exists = false;
                var req = new List<string>();
                if(dataTypeName == "Itinerary Type")
                    tagPath = "itineraryText[]|itineraryTypeId";
                req.Add(tagPath);
                var typeIds = RetrieveValue(req, jsonString);

                if(typeIds.ToList()[0].Value.Count() > 0)
                    exists = true;

                return exists;
            }

        private List<int> GetRegionAndChildRegions(int regionId, List<RegionData> allRegions)
        {
            List<int> allRegionIDs = new List<int>();
            AddChildRegionsToList(allRegionIDs, allRegions, (regionId > 0 ? Enumerable.Repeat(regionId, 1) : allRegions.Where(a => a.ContainRegionid == null).Select(q => q.Id)));

            return allRegionIDs;
        }

        private void AddChildRegionsToList(List<int> finalRegionIDs, List<RegionData> allRegions, IEnumerable<int> parentRegionIds)
        {
            if (parentRegionIds != null && parentRegionIds.Count() > 0)
            {
                finalRegionIDs.AddRange(parentRegionIds);
                List<int> childRegions = GetInnerRegionIDs(allRegions, parentRegionIds);
                AddChildRegionsToList(finalRegionIDs, allRegions, childRegions);
            }
        }

        private List<int> GetInnerRegionIDs(List<RegionData> allRegions, IEnumerable<int> parentRegionIds)
        {
            return allRegions.Where(r => r.ContainRegionid != null && parentRegionIds.Contains((int)r.ContainRegionid)).Select(a => a.Id).ToList();
        }
    }
        
}