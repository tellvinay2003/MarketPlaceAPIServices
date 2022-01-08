using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.TSv2ApiEntities
{
    public class CalculateBookingPriceRequest : BaseRequest
    {   
        public CalculateBookingPriceReq CalculateBookingPriceRequestData { get; set; }
    }

     public class CalculateBookingPriceReq
    {        
        public CalcBookingPriceBasicInfo BasicInfo { get; set; }        
        public CalcBookingPricePassengerInfo PassengerInfo { get; set; }        
        public List<CalcBookingPriceService> ServiceInfo { get; set; }
        public Package_List PackageList { get; set; }
        //public Package_List PackageList { get; set; }
        public CalcBookingPriceModifiers Modifiers { get; set; }
    }
    [Serializable]
    public class Package_List
    {
        public int BookingTypeID { get; set; }
        public int PriceTypeID { get; set; }
        public List<Package_Info> PackageListInfo { get; set; }
    }

    [Serializable]
    public class Package_Info
    {
        public int PackageID { get; set; }
        public int DepartureId { get; set; }
        public System.DateTime? Departre_Date { get; set; }
        public bool IS_NFD { get; set; }
        public List<Package_Main_Element> Package_Main_Elements { get; set; }
        public List<Package_Optional_Element> Package_Optional_Elements { get; set; }
        public Guid MarketplaceProductId { get; set; }
        public Guid SiteId { get; set; }
        public Guid SubscriberId { get; set; }
        public int AgentId { get; set; }
        public bool isAccessNotAllowed { get; set; }

    }

    [Serializable]
    public class Package_Main_Element
    {
        public int Element_ID { get; set; }
        public int Quantity { get; set; }
        public int No_Of_Adults { get; set; }
        public int No_Of_Children { get; set; }
        public Child_Details Child_Details { get; set; }
    }

    [Serializable]
    public class Child_Details
    {
        public List<Age> Ages { get; set; }
    }

    [Serializable]
    public class Age
    {
        public int AGE { get; set; }
        public int Count { get; set; }
        public string ChildIDs { get; set; }
    }

    [Serializable]
    public class Package_Optional_Element
    {
        public int Option_Id { get; set; }
        public DateTime? Option_Date { get; set; }
        public int Quantity { get; set; }
        public int No_of_Adults { get; set; }
        public int No_of_Children { get; set; }
        public Child_Details Child_Details { get; set; }

    }

    public class CalcBookingPriceBasicInfo
    {
        public int? AgentID { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyISOCode { get; set; }
        public int OrganisationId { get; set; }
        public Guid SiteId { get; set; }
        public Guid SubscriberId { get; set; }
    }

    public class CalcBookingPricePassengerInfo
    {
        public List<Adult> AdultInfo { get; set; }
        public List<Child> ChildInfo { get; set; }
    }

    public class CalcBookingPriceService
    {
        public int ServiceID { get; set; }
        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }
        public int BookingTypeID { get; set; }
        public int PriceTypeID { get; set; }
        public List<OptionRequest> OptionInfo { get; set; }
        public List<ExtraRequest> ExtraInfo { get; set; }
        public string AppliedOfferIDs { get; set; }
        public bool EnforceSpecialOffers { get; set; }
        public InstalmentDiscountRequest InstalmentDiscountInfo { get; set; }
        public int BookedServiceID { get; set; } //Savira(110)
        public bool EnforceChildDiscounts { get; set; }
        public string BookedServiceCheckInTime { get; set; }
        public string BookedServiceCheckOutTime { get; set; }
        public int TotalNoOfNights { get; set; }
        public Guid MarketplaceProductId { get; set; }
        public Guid SiteId { get; set; }
        public Guid SubscriberId { get; set; }
        public int AgentId { get; set; }

        public bool isAccessNotAllowed { get; set; }
        public short Producttypeid { get; set; }
    }

    public class CalcBookingPriceModifiers
    {
        public bool ReturnPackageCommission { get; set; }
        public bool Search_By_Bookdate { get; set; }
        public bool ReturnCharterFlightsFromFS { get; set; }
        public bool EvaluateCircuitRule { get; set; }
        public bool ReturnServiceAvailability { get; set; }
        public bool EvaluateRules { get; set; }
    }

    public class CalcBookingPricePax
    {
        public DateTime? Birthdate { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public int LogicalRoomID { get; set; }
    }

    public class Child : CalcBookingPricePax
    {
        public string ChildID { get; set; }
        public int BookedChildRateID { get; set; }
        public int OptionExtraID { get; set; }
        public int BookedOptionID { get; set; }
        public int PassengerTypeID { get; set; }
    }

    public class Adult : CalcBookingPricePax
    {
        public string AdultID { get; set; }
    }

    public class CalcBookingPriceOptionExtra
    {
        public int UniqueIdentifier { get; set; }
        public int Quantity { get; set; }
        public int AdultCount { get; set; }
        public string AdultIDs { get; set; }
        public int? ChildrenCount { get; set; }
        public string ChildrenIDs { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AppliedOfferIDs { get; set; }
        public bool? LCOEnabled { get; set; } 
        public string ChildrenAges { get; set; }
        public int OccupancyTypeID { get; set; }
        public string PriceCode { get; set; }
        public VehicleLocation PickUpLocation { get; set; }
        public VehicleLocation DropOffLocation { get; set; }
        public int BookedOptionID { get; set; }
        public int SellPriceID { get; set; }
        //EOC Aman(217)

    }

    public class OptionRequest : CalcBookingPriceOptionExtra
    {
        public int OptionID { get; set; }
    }

    
    public class ExtraRequest : CalcBookingPriceOptionExtra
    {
        public int ExtraID { get; set; }
    }

    public class VehicleLocation
    {
        public enmLocationTranferType Type;
        public string LocationCode;
        public string Date;
        public string Time;
    }

    public enum enmLocationTranferType
    {
        Airport,
        Station,
        Port,
        Hotel,
        Location
    }

    public class InstalmentDiscountRequest
    {
        public int OrganisationID { get; set; }
        public decimal Discount { get; set; }
        public int InstalmentDiscountPlanDetailID { get; set; }
        public int InstalmentDiscountPlanID { get; set; }
        public int InstalmentDiscountID { get; set; }
        public int NoOfNights { get; set; }
    }

}
