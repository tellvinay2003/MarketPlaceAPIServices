using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MarketPlaceService.DAL.Models;
using System.Threading;

namespace MarketPlaceService.DAL
{
    public class SubscriberRepository : BaseRepository, ISubscriberRepository, IHealthCheck
    {

        public SubscriberRepository(MarketplaceDbContext context) : base(context)
        {

        }
         public async Task<IEnumerable<SubscriberDataModel>> GetSubscribersListAsync()
        {
            IList<SubscriberDataModel> subscriberDataModels = new List<SubscriberDataModel>();            
             subscriberDataModels=(from s in _context.Subscriber 
             join site in _context.Site on s.SiteId equals site.SiteId
             where site.Enabled
             orderby s.SubscriberName
             select new SubscriberDataModel{
                  SubscriberId=s.SubscriberId,
                  SiteId=s.SiteId,
                  OrganizationId=s.OrganizationId,
                  SubscriberName=s.SubscriberName,
                  Enabled=s.Enabled
             }).ToList();

            return await Task.FromResult(subscriberDataModels);
        }

        public async Task<SubscriberDataModel> DeleteSubscriber(Guid id)
        {
          SubscriberDataModel response=new SubscriberDataModel();
          var DeleteSubscribe=_context.Subscriber.Where(s=>s.SubscriberId==id).FirstOrDefault();

          if(DeleteSubscribe ==null)
            return null;
          
          if(DeleteSubscribe.Enabled==true)
          {
            DeleteSubscribe.Enabled=false;
            response.IsValid=true;
            response.Message="Subscriber has been disabled successfully";
          }
          else
          {
            DeleteSubscribe.Enabled=true;
            response.IsValid=true;
            response.Message="Subscriber has been enabled successfully";
          }
            var Suucessid= _context.SaveChanges();
          
          return await Task.FromResult(response);
        }

        public async Task<SubscriberDataModel> EditSubscriber(Guid id)
        {
             var GetSubscriber=(from s in _context.Subscriber where s.SubscriberId==id select new SubscriberDataModel{
               SubscriberId=s.SubscriberId,
               SiteId=s.SiteId,
               OrganizationId=s.OrganizationId,
               SubscriberName=s.SubscriberName,
               Enabled=s.Enabled
             }).FirstOrDefault();             
            return await Task.FromResult(GetSubscriber);
        }
       
       public async Task<SubscriberDataModel> AddNewSubscriber(SubscriberDataModel SubscriberItem)
       {
            var CheckSubsciber=_context.Subscriber.Where(s=>s.SiteId==SubscriberItem.SiteId&&s.OrganizationId==SubscriberItem.OrganizationId&&s.Enabled==true).Select(a=>a).FirstOrDefault();
            if(CheckSubsciber==null)
            {
                var subscriberDataModels = new Subscriber{
                    SubscriberId=Guid.NewGuid(),
                    SiteId=SubscriberItem.SiteId,
                    OrganizationId=SubscriberItem.OrganizationId,
                    SubscriberName=SubscriberItem.SubscriberName,
                    Enabled=true                
                };
                _context.Subscriber.Add(subscriberDataModels);
              _context.SaveChanges();
              SubscriberItem.SubscriberId=subscriberDataModels.SubscriberId;
              SubscriberItem.Enabled=subscriberDataModels.Enabled;
              SubscriberItem.IsValid=true;
            }
            else
            {
              SubscriberItem.IsValid=false;
            }

            return await Task.FromResult(SubscriberItem);
       }

       public async Task<SubscriberDataModel> UpdateSubscriber(SubscriberDataModel SubscriberItem)
       {
           SubscriberDataModel subscriberDataModels = new SubscriberDataModel();
           var CheckSubsciber=_context.Subscriber.Where(s=>s.SiteId==SubscriberItem.SiteId&&s.OrganizationId==SubscriberItem.OrganizationId&&s.Enabled==true&&s.SubscriberId!=SubscriberItem.SubscriberId).Select(a=>a).FirstOrDefault();
           if(CheckSubsciber==null)
           {
            var UpdateSubscriber=_context.Subscriber.Where(s=>s.SubscriberId==SubscriberItem.SubscriberId).FirstOrDefault();
              if(UpdateSubscriber!=null)
              {              
                UpdateSubscriber.SiteId=SubscriberItem.SiteId;
                UpdateSubscriber.OrganizationId=SubscriberItem.OrganizationId;
                UpdateSubscriber.SubscriberName=SubscriberItem.SubscriberName;              
                _context.SaveChanges();
              }
              SubscriberItem.Enabled=UpdateSubscriber.Enabled;
              SubscriberItem.IsValid=true;
            }
            else
            {
              SubscriberItem.IsValid=false;
            }
           return await Task.FromResult(SubscriberItem);
       }

       public async Task<IEnumerable<SubscriberDataModel>> GetEnabledSubscribersListAsync()
       {
         IList<SubscriberDataModel> subscriberDataModels = new List<SubscriberDataModel>();            
             subscriberDataModels=(from s in _context.Subscriber where s.Enabled==true
             select new SubscriberDataModel{
                  SubscriberId=s.SubscriberId,
                  SiteId=s.SiteId,
                  OrganizationId=s.OrganizationId,
                  SubscriberName=s.SubscriberName,
                  Enabled=s.Enabled
             }).ToList();

            return await Task.FromResult(subscriberDataModels);
       }

       public async Task<IEnumerable<SubscriberSupplierDataModel>> GetSupplierMapSubscriberByIdAsync(Guid subscriberId)
       {
         IList<SubscriberSupplierDataModel> subscriberDataModels = new List<SubscriberSupplierDataModel>();            
           
            subscriberDataModels = _context.SubscriberSupplier.Where(ss=>ss.SubscriberId == subscriberId).Select(a=> new SubscriberSupplierDataModel
            {
              SubscriberSupplierId = a.SubscriberSupplierId,
              PublisherId = a.PublisherId,
              SubscriberId = a.SubscriberId,
              SupplierId = a.SupplierId
            }).ToList();
            
            return await Task.FromResult(subscriberDataModels);
       }

       public async Task<SubscriberSupplierDataModel> GetSupplierMapSubscriberByIdandPubIdAsync(Guid subscriberId,Guid publisherId)
       {
         SubscriberSupplierDataModel response=new SubscriberSupplierDataModel();
        
        response =  _context.SubscriberSupplier.Where(ss=>ss.SubscriberId == subscriberId && ss.PublisherId ==publisherId).Select(a=> new SubscriberSupplierDataModel
            {
              SubscriberSupplierId = a.SubscriberSupplierId,
              PublisherId = a.PublisherId,
              SubscriberId = a.SubscriberId,
              SupplierId = a.SupplierId
            }).FirstOrDefault();
         return await Task.FromResult(response);
       }

       public async Task<SubscriberSupplierDataModel> AddNewSubscriberSupplierMap(Guid subscriberId,Guid publisherId,int supplierId)
       {
         SubscriberSupplierDataModel subscriberSupplierDataModel = new SubscriberSupplierDataModel();
          var subscriberSupplier = new SubscriberSupplier{
                SubscriberId = subscriberId,
                PublisherId = publisherId,
                SupplierId = supplierId     
            };
            _context.SubscriberSupplier.Add(subscriberSupplier);
            _context.SaveChanges();

            subscriberSupplierDataModel.PublisherId = publisherId;
            subscriberSupplierDataModel.SubscriberId =  subscriberId;
            subscriberSupplierDataModel.SupplierId = supplierId;
            subscriberSupplierDataModel.SubscriberSupplierId = subscriberSupplier.SubscriberSupplierId;
            
            return await Task.FromResult(subscriberSupplierDataModel);
       }


       public async Task<SubscriberSupplierDataModel> UpdateSubscriberSupplierMap(Guid subscriberId,Guid publisherId,int supplierId)
       {
          SubscriberSupplierDataModel subscriberSupplierDataModel = new SubscriberSupplierDataModel();
          var subscriberSupplier = _context.SubscriberSupplier.FirstOrDefault(ss=> ss.PublisherId == publisherId && ss.SubscriberId == subscriberId);  
          if(subscriberSupplier ==null)
            return null;

          subscriberSupplier.SupplierId = supplierId;
          _context.SubscriberSupplier.Update(subscriberSupplier);
          _context.SaveChanges();

          subscriberSupplierDataModel.PublisherId = publisherId;
          subscriberSupplierDataModel.SubscriberId =  subscriberId;
          subscriberSupplierDataModel.SupplierId = supplierId;
          subscriberSupplierDataModel.SubscriberSupplierId = subscriberSupplier.SubscriberSupplierId;

          return await Task.FromResult(subscriberSupplierDataModel);
       }

       public async Task<bool> DeleteSubscriberSupplierMap(Guid subscriberId,Guid publisherId)
       {
          var subscriberSupplier = _context.SubscriberSupplier.FirstOrDefault(ss=> ss.PublisherId == publisherId && ss.SubscriberId == subscriberId);  
          if(subscriberSupplier ==null)
            return false;

          _context.SubscriberSupplier.Remove(subscriberSupplier);
          _context.SaveChanges();
          return await Task.FromResult(true);
       }

       public async Task<SubscriberChargingPolicyDataModel> GetDefaultSubscriberById(Guid subscriberId)
       {
          SubscriberChargingPolicyDataModel subscriberChargingPolicyDataModel = new SubscriberChargingPolicyDataModel
          {
            DefaultCommunicationType = new CommunicationType(),
            DefaultChargingPolicy = new List<ServiceTypeTypeChargingPolicy>()
          }; 
          
          var subscriberDefault  = _context.SubscriberDefault.FirstOrDefault(a=>a.SubscriberId==subscriberId);
          if(subscriberDefault!=null)
          {
            subscriberChargingPolicyDataModel.DefaultCommunicationType.Id = subscriberDefault.CommunicationTypeId;
          }
          
          subscriberChargingPolicyDataModel.DefaultChargingPolicy = _context.SubscriberChargingPolicy.Where(a=> a.SubscriberId== subscriberId).Select(scp=>  new ServiceTypeTypeChargingPolicy
          {
            ExtraChargingPolicyId = scp.ExtraChargingPolicyId,
            OptionChargingPolicyId = scp.OptionChargingPolicyId,
            ServiceTypeTypeId = scp.ServiceTypeTypeId
          }).ToList();
          subscriberChargingPolicyDataModel.SubscriberId = subscriberId;          
          
          return await Task.FromResult(subscriberChargingPolicyDataModel);
       }

      public async Task<SubscriberChargingPolicyDataModel> UpdateDefaultSubscriber(SubscriberChargingPolicyDataModel request)
      {
         var subscriberChargingPolicy = _context.SubscriberChargingPolicy.Where(scp=>scp.SubscriberId== request.SubscriberId).Select (a=>a).ToList();
         SubscriberChargingPolicyDataModel response=new SubscriberChargingPolicyDataModel();

         var subscriber = _context.Subscriber.FirstOrDefault(a=> a.SubscriberId == request.SubscriberId);
         if(subscriber ==null)
            return null;

          SubscriberDefault subscriberDefault = _context.SubscriberDefault.FirstOrDefault(sd=>sd.SubscriberId==request.SubscriberId);
          if(subscriberDefault !=null)
          {
            subscriberDefault.CommunicationTypeId = request.DefaultCommunicationType.Id;
            _context.SubscriberDefault.Update(subscriberDefault);
          }
          else{
            SubscriberDefault subscriberDefaultreq=new SubscriberDefault();
            subscriberDefaultreq.CommunicationTypeId=request.DefaultCommunicationType!=null?request.DefaultCommunicationType.Id:null;
            subscriberDefaultreq.SubscriberId=request.SubscriberId;
            _context.SubscriberDefault.Add(subscriberDefaultreq);
          }

          List<SubscriberChargingPolicy> subscriberCP = _context.SubscriberChargingPolicy.Where(a=>a.SubscriberId==request.SubscriberId).Select(q=>q).ToList();
          if(subscriberCP.Count!=0)
          {
          foreach(var item in subscriberCP)
          {
              var update = request.DefaultChargingPolicy.FirstOrDefault(a=>a.ServiceTypeTypeId==item.ServiceTypeTypeId);
              item.ExtraChargingPolicyId = update.ExtraChargingPolicyId;
              item.OptionChargingPolicyId = update.OptionChargingPolicyId;
          }
          _context.SubscriberChargingPolicy.UpdateRange(subscriberCP);
          }
          else
          {
            foreach(var item in request.DefaultChargingPolicy)
            {
                SubscriberChargingPolicy chargingPolicy=new SubscriberChargingPolicy();
                chargingPolicy.SubscriberChargingPolicyId=Guid.NewGuid();
                chargingPolicy.ServiceTypeTypeId=item.ServiceTypeTypeId;
                chargingPolicy.SubscriberId=request.SubscriberId;
                chargingPolicy.ExtraChargingPolicyId = item.ExtraChargingPolicyId;
                chargingPolicy.OptionChargingPolicyId = item.OptionChargingPolicyId;
                _context.SubscriberChargingPolicy.AddRange(chargingPolicy);
            }
          }
          _context.SaveChanges();
        

        return await Task.FromResult(request);
      }

      public async Task<SubscriberDefaultDataModel> GetContractDefaultSubscriberById(Guid subscriberId)
       {
          SubscriberDefaultDataModel subscriberDefaultDataModel = new SubscriberDefaultDataModel();            
          var subscriberDefault = _context.SubscriberDefault.FirstOrDefault(a=>a.SubscriberId==subscriberId);

          if(subscriberDefault != null)
          {
            subscriberDefaultDataModel.SeasonTypeID = subscriberDefault.SeasonTypeId;
            subscriberDefaultDataModel.BuyBookingTypeID = subscriberDefault.BuyBookingTypeId;
            subscriberDefaultDataModel.BuyPriceTypeID = subscriberDefault.BuyPriceTypeId;
            subscriberDefaultDataModel.StartDateOffsetDays = subscriberDefault.Startdateoffsetdays ?? 0;
            subscriberDefaultDataModel.EndDateOffsetDays = subscriberDefault.Enddateoffsetdays ?? 3650;
          }
          return await Task.FromResult(subscriberDefaultDataModel);
       }

      public async Task<SubscriberDefaultDataModel> UpdateContractDefaultSubscriber(SubscriberDefaultDataModel request)
      {
        SubscriberDefaultDataModel response=new SubscriberDefaultDataModel();
        var subscriber = _context.Subscriber.FirstOrDefault(a=> a.SubscriberId == request.SubscriberId);
         if(subscriber == null)
            return null;

        var subscriberDefault = _context.SubscriberDefault.FirstOrDefault(a=>a.SubscriberId==request.SubscriberId);
        if(subscriberDefault!=null)
        {
            subscriberDefault.BuyBookingTypeId = request.BuyBookingTypeID;
            subscriberDefault.BuyPriceTypeId = request.BuyPriceTypeID;
            subscriberDefault.SeasonTypeId = request.SeasonTypeID;
            subscriberDefault.Startdateoffsetdays = request.StartDateOffsetDays ?? 0;
            subscriberDefault.Enddateoffsetdays = request.EndDateOffsetDays ?? 3650;
            _context.SubscriberDefault.Update(subscriberDefault);         
        }
        else
        {
          var subscriberDefaultDataModels = new SubscriberDefault{
                SubscriberDefaultId=Guid.NewGuid(),
                SubscriberId=request.SubscriberId,
                SeasonTypeId=request.SeasonTypeID,
                BuyBookingTypeId=request.BuyBookingTypeID,
                BuyPriceTypeId=request.BuyPriceTypeID,
                Startdateoffsetdays = request.StartDateOffsetDays ?? 0,
                Enddateoffsetdays = request.EndDateOffsetDays ?? 3650
            };
          _context.SubscriberDefault.Add(subscriberDefaultDataModels);         
        }
         _context.SaveChanges();

        return await Task.FromResult(request);
      }

      public async Task<SubscriberDataModel> GetSubscriberById(Guid id)
      {
        SubscriberDataModel response=new SubscriberDataModel();
        response=(from s in _context.Subscriber where s.SubscriberId==id select new SubscriberDataModel{
          SubscriberId = s.SubscriberId,
          SiteId=s.SiteId,
          OrganizationId = s.OrganizationId
        }).FirstOrDefault();
        return await Task.FromResult(response);
      }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SubscriberDefaultsProductCode>> GetSubscriberDefaultsProductCodes(Guid subscriberId)
        {
            List<SubscriberDefaultsProductCode> response = _context.SubscriberProductCode.Where(a=>a.SubscriberId==subscriberId).Select(q=> new SubscriberDefaultsProductCode
            {
                RuleId=q.SubscriberProductCodeId,
                SubscriberId = subscriberId,
                Region = q.RegionId,
                ApplyToExtras = q.ApplytoExtras == null ? true : (bool)q.ApplytoExtras,
                ApplyToOptions = q.ApplytoOptions == null ? true : (bool)q.ApplytoOptions,
                ProductCodeId = q.ProductCodeId,
                AllServiceTypesSelected = q.Allservicetypes,
                ServiceTypes = q.SubscriberProductCodeServiceType.Select(p=> p.ServiceTypeId).ToList()
            }).ToList();

            return response;
        }

        public async Task<SubscriberDefaultsProductCode> GetSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId)
        {
            SubscriberDefaultsProductCode response = _context.SubscriberProductCode.Where(a=>a.SubscriberId==subscriberId && a.SubscriberProductCodeId ==ruleId).Select(q=> new SubscriberDefaultsProductCode
            {
                RuleId=q.SubscriberProductCodeId,
                SubscriberId = subscriberId,
                Region = q.RegionId,
                ApplyToExtras = q.ApplytoExtras == null ? true : (bool)q.ApplytoExtras,
                ApplyToOptions = q.ApplytoOptions == null ? true : (bool)q.ApplytoOptions,
                ProductCodeId = q.ProductCodeId,
                AllServiceTypesSelected = q.Allservicetypes,
                ServiceTypes = q.SubscriberProductCodeServiceType.Select(p=> p.ServiceTypeId).ToList()
            }).FirstOrDefault();

            return response;
        }

        public async Task<SubscriberDefaultsProductCode> InsertSubscriberDefaultsProductCodes(Guid subscriberId, SubscriberDefaultsProductCode request)
        {
            SubscriberProductCode productCode = new SubscriberProductCode
            {
                ApplytoExtras = request.ApplyToExtras,
                ApplytoOptions = request.ApplyToOptions,
                ProductCodeId = request.ProductCodeId,
                RegionId = request.Region,
                SubscriberId = request.SubscriberId,
                Allservicetypes = request.AllServiceTypesSelected,
                SubscriberProductCodeServiceType = request.ServiceTypes.Select(a => new SubscriberProductCodeServiceType
                {                    
                    ServiceTypeId = a
                }).ToList()
            };
            _context.SubscriberProductCode.Add(productCode);
            _context.SaveChanges();
            request.RuleId = productCode.SubscriberProductCodeId;
            return request;
        }

        public async Task<SubscriberDefaultsProductCode> UpdateSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId, SubscriberDefaultsProductCode request)
        {
            //first remove the service types that were previously saved
            List<SubscriberProductCodeServiceType> serviceTypes = _context.SubscriberProductCodeServiceType.Where(a => a.SubscriberProductCodeId == ruleId).Select(q => q).ToList();
            SubscriberProductCode productCode = _context.SubscriberProductCode.FirstOrDefault(spc => spc.SubscriberProductCodeId == ruleId && spc.SubscriberId == subscriberId);
            productCode.ApplytoExtras = request.ApplyToExtras;
            productCode.ApplytoOptions = request.ApplyToOptions;
            productCode.ProductCodeId = request.ProductCodeId;
            productCode.RegionId = request.Region;
            productCode.Allservicetypes=request.AllServiceTypesSelected;

            _context.SubscriberProductCodeServiceType.RemoveRange(serviceTypes);
            //add new service types
            List<SubscriberProductCodeServiceType> newServiceTypes = request.ServiceTypes.Select(a => new SubscriberProductCodeServiceType
            {
                ServiceTypeId = a,
                SubscriberProductCodeId = ruleId
            }).ToList();
            _context.SubscriberProductCodeServiceType.AddRange(newServiceTypes);
            _context.SubscriberProductCode.Update(productCode);
            _context.SaveChanges();
            return request;
        }
        public async Task<bool> DeleteSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId)
        {
            List<SubscriberProductCodeServiceType> serviceTypes = _context.SubscriberProductCodeServiceType.Where(a => a.SubscriberProductCodeId == ruleId).Select(q => q).ToList();
            SubscriberProductCode productCode = _context.SubscriberProductCode.FirstOrDefault(spc => spc.SubscriberProductCodeId == ruleId && spc.SubscriberId == subscriberId);
            _context.SubscriberProductCodeServiceType.RemoveRange(serviceTypes);
            _context.SubscriberProductCode.Remove(productCode);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<SubscriberDefaultSellingPrice>> GetSubscriberDefaultSellPrices(Guid subscriberId)
        {
            List<SubscriberDefaultSellingPrice> response = _context.SubscriberDefaultSellingPrices.Where(a=>a.Subscriberid==subscriberId).Select(q=> new SubscriberDefaultSellingPrice
            {
                RuleId=q.Subscriberdefaultsellingpriceid,
                SubscriberId = subscriberId,
                RegionId = q.Regionid,
                Sequence=q.Sequence,
                AllServiceTypesSelected = q.Allservicetypes,
                ServiceTypes=_context.SubscriberDefaultSellingPriceServiceType.Where(a=>a.Subscriberdefaultsellingpriceid==q.Subscriberdefaultsellingpriceid).Select(a=>a.Servicetypeid).ToList(),
                SubscriberDefaultSellingPricePolicy=_context.SubscriberDefaultSellingPricePolicies.Where(a=>a.Subscriberdefaultsellingpriceid==q.Subscriberdefaultsellingpriceid).Select(x=>new SubscriberDefaultSellingPricePolicy{
                   SubscriberDefaultSellingPricePolicyId=x.Subscriberdefaultsellingpricepolicyid,
                   BookingTypeId=x.Bookingtypeid,
                   PriceTypeId=x.Pricetypeid,
                   TaxId=x.Taxid,
                }).ToList(),
            }).OrderBy(p=>p.Sequence).ToList();
            return await Task.FromResult(response);
        }

        public async Task<SubscriberDefaultSellingPrice> GetSubscriberDefaultSellPriceById(Guid subscriberId, int ruleId)
        {
           SubscriberDefaultSellingPrice response = _context.SubscriberDefaultSellingPrices.Where(a=>a.Subscriberid==subscriberId && a.Subscriberdefaultsellingpriceid==ruleId).Select(q=> new SubscriberDefaultSellingPrice
            {
                RuleId=q.Subscriberdefaultsellingpriceid,
                SubscriberId = subscriberId,
                RegionId = q.Regionid,
                Sequence=q.Sequence,
                AllServiceTypesSelected=q.Allservicetypes,
                ServiceTypes=_context.SubscriberDefaultSellingPriceServiceType.Where(a=>a.Subscriberdefaultsellingpriceid==q.Subscriberdefaultsellingpriceid).Select(a=>a.Servicetypeid).ToList(),
                SubscriberDefaultSellingPricePolicy=_context.SubscriberDefaultSellingPricePolicies.Where(a=>a.Subscriberdefaultsellingpriceid==q.Subscriberdefaultsellingpriceid).Select(x=>new SubscriberDefaultSellingPricePolicy{
                   SubscriberDefaultSellingPricePolicyId=x.Subscriberdefaultsellingpricepolicyid,
                   BookingTypeId=x.Bookingtypeid,
                   PriceTypeId=x.Pricetypeid,
                   TaxId=x.Taxid,
                }).ToList(),
            }).FirstOrDefault();
            return await Task.FromResult(response);
        }
       
       public  async Task<SubscriberDefaultSellingPrice> InsertSubscriberDefaultsSellPrice(Guid subscriberId, SubscriberDefaultSellingPrice request)
       {
           SubscriberDefaultSellingPrices sellingPrice = new SubscriberDefaultSellingPrices
            {
                Subscriberid = subscriberId,
                Regionid = request.RegionId == 0 ? null : request.RegionId,
                Sequence = request.Sequence,
                Allservicetypes = request.AllServiceTypesSelected,
                SubscriberDefaultSellingPriceServiceType = request.ServiceTypes.Select(a => new SubscriberDefaultSellingPriceServiceType
                 {
                    Servicetypeid = a
                 }).ToList(),      

                SubscriberDefaultSellingPricePolicies= request.SubscriberDefaultSellingPricePolicy.Select(a=>new SubscriberDefaultSellingPricePolicies
                {
                    Bookingtypeid=a.BookingTypeId,
                    Pricetypeid=a.PriceTypeId,
                    Taxid=a.TaxId
                }).ToList()                       
            };
             _context.SubscriberDefaultSellingPrices.Add(sellingPrice);
             _context.SaveChanges();           
             request.RuleId=sellingPrice.Subscriberdefaultsellingpriceid;              
             request.SubscriberDefaultSellingPricePolicy=sellingPrice.SubscriberDefaultSellingPricePolicies.Select(x=>new SubscriberDefaultSellingPricePolicy{
               SubscriberDefaultSellingPricePolicyId=x.Subscriberdefaultsellingpricepolicyid,
               BookingTypeId=x.Bookingtypeid,
               PriceTypeId=x.Pricetypeid,
               TaxId=x.Taxid
             }).ToList(); 

            
             return await Task.FromResult(request);
       }

       public async Task<SubscriberDefaultSellingPrice> UpdateSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId, SubscriberDefaultSellingPrice request)
       {
            List<SubscriberDefaultSellingPriceServiceType> serviceTypes = _context.SubscriberDefaultSellingPriceServiceType.Where(a => a.Subscriberdefaultsellingpriceid == ruleId).Select(q => q).ToList();
            List<SubscriberDefaultSellingPricePolicies> sellingPricePolicies=_context.SubscriberDefaultSellingPricePolicies.Where(a=>a.Subscriberdefaultsellingpriceid == ruleId).Select(q=>q).ToList();
            SubscriberDefaultSellingPrices sellingPrices = _context.SubscriberDefaultSellingPrices.FirstOrDefault(sp => sp.Subscriberdefaultsellingpriceid == ruleId && sp.Subscriberid == subscriberId);
            sellingPrices.Regionid = request.RegionId == 0 ? null : request.RegionId;
            sellingPrices.Sequence=request.Sequence;
            sellingPrices.Allservicetypes=request.AllServiceTypesSelected;

            _context.SubscriberDefaultSellingPriceServiceType.RemoveRange(serviceTypes);
           // _context.SubscriberDefaultSellingPricePolicies.RemoveRange(sellingPricePolicies);
            //add new service types
            List<SubscriberDefaultSellingPriceServiceType> newServiceTypes = request.ServiceTypes.Select(a => new SubscriberDefaultSellingPriceServiceType
            {
                Servicetypeid = a,
                Subscriberdefaultsellingpriceid = ruleId
            }).ToList();

            List<SubscriberDefaultSellingPricePolicies> newSellingPricePolicies =request.SubscriberDefaultSellingPricePolicy.Select(a=>new SubscriberDefaultSellingPricePolicies{
                Subscriberdefaultsellingpriceid=ruleId,
                Bookingtypeid=a.BookingTypeId,
                Pricetypeid=a.PriceTypeId,
                Taxid=a.TaxId
            }).ToList();

            var removeitems= sellingPricePolicies.Where(a=>!request.SubscriberDefaultSellingPricePolicy.Any(x=>x.SubscriberDefaultSellingPricePolicyId==a.Subscriberdefaultsellingpricepolicyid)).ToList();
            if(removeitems.Count>0)
            _context.SubscriberDefaultSellingPricePolicies.RemoveRange(removeitems);

            var newitems=request.SubscriberDefaultSellingPricePolicy.Where(a=>a.SubscriberDefaultSellingPricePolicyId==0).ToList();
            if(newitems.Count>0)
            foreach(var item in newitems)
            {
               var itm =new SubscriberDefaultSellingPricePolicies();
               itm.Bookingtypeid=item.BookingTypeId;
               itm.Pricetypeid=item.PriceTypeId;
               itm.Taxid=item.TaxId;
               itm.Subscriberdefaultsellingpriceid=ruleId;
               _context.SubscriberDefaultSellingPricePolicies.Add(itm);
            }
            

            _context.SubscriberDefaultSellingPriceServiceType.AddRange(newServiceTypes);
           // _context.SubscriberDefaultSellingPricePolicies.AddRange(newSellingPricePolicies);
            _context.SubscriberDefaultSellingPrices.Update(sellingPrices);
            _context.SaveChanges();
            request.SubscriberDefaultSellingPricePolicy=_context.SubscriberDefaultSellingPricePolicies.Where(a=>a.Subscriberdefaultsellingpriceid==ruleId).Select(x=>new SubscriberDefaultSellingPricePolicy{
               SubscriberDefaultSellingPricePolicyId=x.Subscriberdefaultsellingpricepolicyid,
               BookingTypeId=x.Bookingtypeid,
               PriceTypeId=x.Pricetypeid,
               TaxId=x.Taxid
             }).ToList(); 
          return await Task.FromResult(request);
       }

       public async Task<bool> DeleteSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId)
       {
            List<SubscriberDefaultSellingPriceServiceType> serviceTypes = _context.SubscriberDefaultSellingPriceServiceType.Where(a => a.Subscriberdefaultsellingpriceid == ruleId).Select(q => q).ToList();
            List<SubscriberDefaultSellingPricePolicies> sellingPricePolicies = _context.SubscriberDefaultSellingPricePolicies.Where(a => a.Subscriberdefaultsellingpriceid == ruleId).Select(q => q).ToList();
            SubscriberDefaultSellingPrices sellingPrice = _context.SubscriberDefaultSellingPrices.FirstOrDefault(sp => sp.Subscriberdefaultsellingpriceid == ruleId && sp.Subscriberid == subscriberId);
            _context.SubscriberDefaultSellingPriceServiceType.RemoveRange(serviceTypes);
            _context.SubscriberDefaultSellingPricePolicies.RemoveRange(sellingPricePolicies);
            _context.SubscriberDefaultSellingPrices.Remove(sellingPrice);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
       }

    }
}
