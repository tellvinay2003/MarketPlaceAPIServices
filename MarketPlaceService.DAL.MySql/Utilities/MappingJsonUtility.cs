using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.DAL.Utilities;

namespace MarketPlaceService.DAL.Utilities
{
    public class MappingJsonUtility : BaseRepository, IMappingJsonUtility
    {
         public MappingJsonUtility(MarketplaceDbContext context): base(context)
        {
           
        }
        public  IEnumerable<MessageFieldDetails> GetMessageFieldDetails(int MessageTypeID)
        {
            var result = (from messageField in _context.MessageFields 
            join masterDataType in _context.MasterDataTypes 
            on messageField.Mappingdatatype equals masterDataType.Datatypeid
            where messageField.Messagetypeid == MessageTypeID
            select new MessageFieldDetails{
                FieldName = messageField.Fieldname,
                FieldPath = messageField.Fieldpath,
                MappingDataType = messageField.Mappingdatatype,
                IsMappingMandatory = messageField.Ismappingmandatory,
                DataTypeName = masterDataType.Datatypename,
                RemoveTag = messageField.Removetag
            }).ToList();
            
            return result;
        }

        public  IEnumerable<ReplaceTagsResponse> GetReplaceTags(GetReplaceTagsRequest request)
        {
            var allData = (from mappingData in _context.MappingData
                where request.messageFields.Select(a=> a.MappingDataType).ToList().Contains(mappingData.Datatypeid) && mappingData.Mappingdirectionid == request.mappingDirection 
                && mappingData.Siteid == request.siteId
                select mappingData).ToList();
               
            var replaceTags =  (from mappingData in allData               
                group mappingData by new
                {
                    mappingData.Datatypeid
                } into mappingDataGroup                
                select new ReplaceTagsResponse
                {        
                    DataTypeId = mappingDataGroup.Key.Datatypeid,            
                    MappingValues = mappingDataGroup.Select(q=> new MappingList
                    {
                        SourceValueId = q.Sourceid,
                        TagetValueId = q.Targetid,
                        SourceValueName = q.Sourcename,
                        TagetValueName = q.Targetname
                    }).ToList()                    
                }).ToList();

            foreach(var t in request.messageFields)
            {
                var replaceTag = replaceTags.FirstOrDefault(a=> a.DataTypeId == t.MappingDataType);
                if(replaceTag != null)
                {
                    replaceTag.TagFullPath = t.FieldPath;
                    replaceTag.TagName = t.FieldName;
                    replaceTag.IsMappingMandatory = t.IsMappingMandatory;
                    replaceTag.DataTypeName = t.DataTypeName;
                    replaceTag.RemoveTag = t.RemoveTag;
                }
                else
                {
                    replaceTags.Add( new ReplaceTagsResponse{
                        TagFullPath = t.FieldPath,
                        TagName = t.FieldName,
                        IsMappingMandatory = t.IsMappingMandatory,
                        DataTypeId = t.MappingDataType,
                        MappingValues = new List<MappingList>(),
                        DataTypeName = t.DataTypeName,
                        RemoveTag = t.RemoveTag
                    });
                }
               
            }
    
            return replaceTags;
        }

        public SubscriberDefaultsModelForSubscription GetSubscriberDefaults(Guid subscriberId, Guid marketplaceProductId)
        {
            SubscriberDefaultsModelForSubscription response = new SubscriberDefaultsModelForSubscription();

            var chargingPolicies = (from scp in _context.SubscriberChargingPolicy
                                    where scp.SubscriberId == subscriberId
                                    select new ServiceTypeTypeChargingPolicy{
                                       ServiceTypeTypeId = scp.ServiceTypeTypeId,
                                       OptionChargingPolicyId = scp.OptionChargingPolicyId,
                                       ExtraChargingPolicyId = scp.ExtraChargingPolicyId
                                    }).ToList();

            response.ChargingPolicy = new SubscriberChargingPolicyDataModel();
            response.ChargingPolicy.DefaultChargingPolicy = new List<ServiceTypeTypeChargingPolicy>();
            response.ChargingPolicy.DefaultChargingPolicy = chargingPolicies;

            var defaults = (from sd in _context.SubscriberDefault
                            where sd.SubscriberId == subscriberId
                            select new SubscriberDefaultDataModel{
                                SeasonTypeID = sd.SeasonTypeId,
                                BuyPriceTypeID = sd.BuyPriceTypeId,
                                BuyBookingTypeID = sd.BuyBookingTypeId
                            }).FirstOrDefault();

            response.Defaults = new SubscriberDefaultDataModel();
            response.Defaults.SeasonTypeID = defaults.SeasonTypeID;
            response.Defaults.BuyPriceTypeID = defaults.BuyPriceTypeID;
            response.Defaults.BuyBookingTypeID = defaults.BuyBookingTypeID;

            response.ChargingPolicy.DefaultCommunicationType = new CommunicationType();
            response.ChargingPolicy.DefaultCommunicationType.Id = _context.SubscriberDefault.FirstOrDefault(a=>a.SubscriberId == subscriberId).CommunicationTypeId;

            response.Suppliers = (from mp in _context.MarketplaceProduct
                                    join pp in _context.PublishedProducts on mp.Publishedproductid equals pp.PublishedProductId
                                    join ss in _context.SubscriberSupplier on pp.PublisherId equals ss.PublisherId
                                    where mp.Marketplaceproductid == marketplaceProductId && 
                                            ss.SubscriberId == subscriberId 
                                            select new SubscriberSupplierDataModel{
                                                SupplierId = ss.SupplierId,
                                                SubscriberId = ss.SubscriberId
                                            }).ToList();


            return response;
        }
    }
}
