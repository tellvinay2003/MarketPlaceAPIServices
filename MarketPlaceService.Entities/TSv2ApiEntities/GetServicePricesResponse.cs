using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities.TSv2ApiEntities
{
     [Serializable]
    public class GetServicePricesResponse
    {

        private RequestResponseError[] errorsField;

        private PriceAndAvailabilityWarnings[] warningsField;

        private GeoRegion[] geoLocationsField;

        private List<PriceAndAvailabilityService> servicesField;

        private Facility[] facilitiesField;

        private ServiceTypeRatingType[] serviceTypeRatingTypesField;

        private UIPricesAndAvailabilityRequest[] arrayOfUIPricesAndAvailabilityRequestField;

        private bool bestSellerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error")]
        public RequestResponseError[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Warning")]
        public PriceAndAvailabilityWarnings[] Warnings
        {
            get
            {
                return this.warningsField;
            }
            set
            {
                this.warningsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Region", IsNullable = false)]
        public GeoRegion[] GeoLocations
        {
            get
            {
                return this.geoLocationsField;
            }
            set
            {
                this.geoLocationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        public List<PriceAndAvailabilityService> Services
        {
            get
            {
                return this.servicesField;
            }
            set
            {
                this.servicesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        public Facility[] Facilities
        {
            get
            {
                return this.facilitiesField;
            }
            set
            {
                this.facilitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        public ServiceTypeRatingType[] ServiceTypeRatingTypes
        {
            get
            {
                return this.serviceTypeRatingTypesField;
            }
            set
            {
                this.serviceTypeRatingTypesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        public UIPricesAndAvailabilityRequest[] ArrayOfUIPricesAndAvailabilityRequest
        {
            get
            {
                return this.arrayOfUIPricesAndAvailabilityRequestField;
            }
            set
            {
                this.arrayOfUIPricesAndAvailabilityRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public bool BestSeller
        {
            get
            {
                return this.bestSellerField;
            }
            set
            {
                this.bestSellerField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityWarnings
    {

        private string descriptionField;

        private System.DateTime warningDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime WarningDate
        {
            get
            {
                return this.warningDateField;
            }
            set
            {
                this.warningDateField = value;
            }
        }
    }
    public partial class GeoRegion
    {

        private string idField;

        private string nameField;

        private string levelField;

        private string latitiudeField;

        private string longitudeField;

        private string parentRegionIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Latitiude
        {
            get
            {
                return this.latitiudeField;
            }
            set
            {
                this.latitiudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string ParentRegionID
        {
            get
            {
                return this.parentRegionIDField;
            }
            set
            {
                this.parentRegionIDField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityService
    {

        private PriceAndAvailabilityWarnings[] warningsField;

        private string supplierNameField;

        private string serviceIDField;

        private string serviceCodeField;

        private string starRatingField;

        private string availableField;

        private string currencyField;

        private bool isRecommendedProductField;

        private enumCancellationPolicyIndicator cancellationPolicyIndicatorField;

        private string ePClientCancellationPolicyFlagField;

        private AppliedTaxeList aPPLIED_TAXField;

        private PriceAndAvailabilityResponseServiceOption[] serviceOptionsField;

        private int[] serviceFacilitiesField;

        private RatingType[] serviceRatingTypesField;

        private ServiceTermsDetails[] serviceTermsField;

        private PriceAndAvailabilityResponseMeals mealsField;

        private string textField;

        private bool isEstimatePriceField;

        private MatchCode matchCodeField;

        private ServiceInfo serviceInformationField;

        private int pickUpOptionIDField;

        private int dropOffOptionIDField;

        private string startingPriceField;

        private string serviceStatusNameField;

        private string locationField;

        private string imageIDsField;

        private Additional_Prices[] additionalPricesField;

        private bool isEstimatedServiceField;

        private int locationIdField;

        private string sellingPriceField;

        private string buyingPriceField;

        private Notes[] notesField;

        private bool isRuleAppliedField;
        public Guid MarketplaceProductId {get;set;}
        public Guid SubscriberId { get; set; }
         
        public bool IsAllocationAvailable {get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Warning")]
        public PriceAndAvailabilityWarnings[] Warnings
        {
            get
            {
                return this.warningsField;
            }
            set
            {
                this.warningsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string SupplierName
        {
            get
            {
                return this.supplierNameField;
            }
            set
            {
                this.supplierNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ServiceID
        {
            get
            {
                return this.serviceIDField;
            }
            set
            {
                this.serviceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ServiceCode
        {
            get
            {
                return this.serviceCodeField;
            }
            set
            {
                this.serviceCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string StarRating
        {
            get
            {
                return this.starRatingField;
            }
            set
            {
                this.starRatingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Available
        {
            get
            {
                return this.availableField;
            }
            set
            {
                this.availableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public bool IsRecommendedProduct
        {
            get
            {
                return this.isRecommendedProductField;
            }
            set
            {
                this.isRecommendedProductField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public enumCancellationPolicyIndicator CancellationPolicyIndicator
        {
            get
            {
                return this.cancellationPolicyIndicatorField;
            }
            set
            {
                this.cancellationPolicyIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string EPClientCancellationPolicyFlag
        {
            get
            {
                return this.ePClientCancellationPolicyFlagField;
            }
            set
            {
                this.ePClientCancellationPolicyFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public AppliedTaxeList APPLIED_TAX
        {
            get
            {
                return this.aPPLIED_TAXField;
            }
            set
            {
                this.aPPLIED_TAXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        public PriceAndAvailabilityResponseServiceOption[] ServiceOptions
        {
            get
            {
                return this.serviceOptionsField;
            }
            set
            {
                this.serviceOptionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 12)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ID", IsNullable = false)]
        public int[] ServiceFacilities
        {
            get
            {
                return this.serviceFacilitiesField;
            }
            set
            {
                this.serviceFacilitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ServiceRatingTypes", Order = 13)]
        public RatingType[] ServiceRatingTypes
        {
            get
            {
                return this.serviceRatingTypesField;
            }
            set
            {
                this.serviceRatingTypesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 14)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ServiceTerm")]
        public ServiceTermsDetails[] ServiceTerms
        {
            get
            {
                return this.serviceTermsField;
            }
            set
            {
                this.serviceTermsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public PriceAndAvailabilityResponseMeals Meals
        {
            get
            {
                return this.mealsField;
            }
            set
            {
                this.mealsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public bool IsEstimatePrice
        {
            get
            {
                return this.isEstimatePriceField;
            }
            set
            {
                this.isEstimatePriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public MatchCode MatchCode
        {
            get
            {
                return this.matchCodeField;
            }
            set
            {
                this.matchCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public ServiceInfo ServiceInformation
        {
            get
            {
                return this.serviceInformationField;
            }
            set
            {
                this.serviceInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public int PickUpOptionID
        {
            get
            {
                return this.pickUpOptionIDField;
            }
            set
            {
                this.pickUpOptionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public int DropOffOptionID
        {
            get
            {
                return this.dropOffOptionIDField;
            }
            set
            {
                this.dropOffOptionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string StartingPrice
        {
            get
            {
                return this.startingPriceField;
            }
            set
            {
                this.startingPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string ServiceStatusName
        {
            get
            {
                return this.serviceStatusNameField;
            }
            set
            {
                this.serviceStatusNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public string ImageIDs
        {
            get
            {
                return this.imageIDsField;
            }
            set
            {
                this.imageIDsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 26)]
        public Additional_Prices[] AdditionalPrices
        {
            get
            {
                return this.additionalPricesField;
            }
            set
            {
                this.additionalPricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public bool IsEstimatedService
        {
            get
            {
                return this.isEstimatedServiceField;
            }
            set
            {
                this.isEstimatedServiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public int LocationId
        {
            get
            {
                return this.locationIdField;
            }
            set
            {
                this.locationIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public string SellingPrice
        {
            get
            {
                return this.sellingPriceField;
            }
            set
            {
                this.sellingPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 30)]
        public string BuyingPrice
        {
            get
            {
                return this.buyingPriceField;
            }
            set
            {
                this.buyingPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 31)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Note")]
        public Notes[] Notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 32)]
        public bool IsRuleApplied
        {
            get
            {
                return this.isRuleAppliedField;
            }
            set
            {
                this.isRuleAppliedField = value;
            }
        }
    }
    public partial class Facility : BaseType
    {
    }
    public partial class ServiceTypeRatingType : BaseType
    {

        private ServiceTypeRating[] serviceTypeRatingsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ServiceTypeRating[] ServiceTypeRatings
        {
            get
            {
                return this.serviceTypeRatingsField;
            }
            set
            {
                this.serviceTypeRatingsField = value;
            }
        }
    }
    public partial class BaseType
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }
    public partial class UIPricesAndAvailabilityRequest
    {

        private string serviceCodeField;

        private int serviceIDField;

        private int serviceTypeIDField;

        private bool isNonHotelTypeField;

        private bool availableOnlyField;

        private System.DateTime bookingStartDateField;

        private System.DateTime bookingEndDateField;

        private short noOfNightsField;

        private enmRoom_Reply room_ReplyField;

        private RoomDetails[] roomDetailsField;

        private InsuranceDetails insuranceDetailsField;

        private string currencyField;

        private bool requireEstimatedPricesField;

        private int bookingTypeField;

        private int priceTypeField;

        private string bookingRefNoField;

        private bool isFastBuildServiceField;

        private int leadPassengerCountryIDField;

        private bool isEstimatedServiceField;
        public Guid MarketplaceProductId {get;set;}
        public Guid SubscriberId { get; set; }        
        public bool HasAccess { get; set; }
        public Guid SiteId { get; set; }
        public int ClientId { get; set; }
        public short Producttypeid { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ServiceCode
        {
            get
            {
                return this.serviceCodeField;
            }
            set
            {
                this.serviceCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int ServiceID
        {
            get
            {
                return this.serviceIDField;
            }
            set
            {
                this.serviceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int ServiceTypeID
        {
            get
            {
                return this.serviceTypeIDField;
            }
            set
            {
                this.serviceTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool IsNonHotelType
        {
            get
            {
                return this.isNonHotelTypeField;
            }
            set
            {
                this.isNonHotelTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool AvailableOnly
        {
            get
            {
                return this.availableOnlyField;
            }
            set
            {
                this.availableOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public System.DateTime BookingStartDate
        {
            get
            {
                return this.bookingStartDateField;
            }
            set
            {
                this.bookingStartDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public System.DateTime BookingEndDate
        {
            get
            {
                return this.bookingEndDateField;
            }
            set
            {
                this.bookingEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public short NoOfNights
        {
            get
            {
                return this.noOfNightsField;
            }
            set
            {
                this.noOfNightsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public enmRoom_Reply Room_Reply
        {
            get
            {
                return this.room_ReplyField;
            }
            set
            {
                this.room_ReplyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 9)]
        public RoomDetails[] RoomDetails
        {
            get
            {
                return this.roomDetailsField;
            }
            set
            {
                this.roomDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public InsuranceDetails InsuranceDetails
        {
            get
            {
                return this.insuranceDetailsField;
            }
            set
            {
                this.insuranceDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool RequireEstimatedPrices
        {
            get
            {
                return this.requireEstimatedPricesField;
            }
            set
            {
                this.requireEstimatedPricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public int BookingType
        {
            get
            {
                return this.bookingTypeField;
            }
            set
            {
                this.bookingTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public int PriceType
        {
            get
            {
                return this.priceTypeField;
            }
            set
            {
                this.priceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string BookingRefNo
        {
            get
            {
                return this.bookingRefNoField;
            }
            set
            {
                this.bookingRefNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public bool IsFastBuildService
        {
            get
            {
                return this.isFastBuildServiceField;
            }
            set
            {
                this.isFastBuildServiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public int LeadPassengerCountryID
        {
            get
            {
                return this.leadPassengerCountryIDField;
            }
            set
            {
                this.leadPassengerCountryIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public bool IsEstimatedService
        {
            get
            {
                return this.isEstimatedServiceField;
            }
            set
            {
                this.isEstimatedServiceField = value;
            }
        }
    }
    public enum enumCancellationPolicyIndicator
    {

        /// <remarks/>
        OnDemand,

        /// <remarks/>
        Provided,

        /// <remarks/>
        UnAvailable,
    }
    public partial class AppliedTaxeList
    {

        private double tAXField;

        private double cOSTTAXField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public double TAX
        {
            get
            {
                return this.tAXField;
            }
            set
            {
                this.tAXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public double COSTTAX
        {
            get
            {
                return this.cOSTTAXField;
            }
            set
            {
                this.cOSTTAXField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityResponseServiceOption
    {

        private PriceAndAvailabilityWarnings[] warningsField;

        private int minAdultField;

        private string totalGrossSellingPriceField;

        private string totalSellingPriceField;

        private string totalTaxField;

        private AppliedTaxeList aPPLIED_TAXField;

        private string totalBuyingPriceField;

        private string currencyField;

        private string endPointCurrencyField;

        private string totalEndPointCostField;

        private int maxAdultField;

        private int minChildField;

        private int maxChildField;

        private int occupancyField;

        private OptionOccupancy optionOccupancyField;

        private PriceAndAvailabilityResponsePricing[] pricesField;

        private string priceCodeField;

        private string ruleTextField;

        private double commissionAmountField;

        private RULE[] aPPLIED_RULESField;

        private int bookingTypeIDField;

        private int priceTypeIDField;

        private AttachedOptionExtra attachedOptionExtraField;

        private string vehicleTypeField;

        private string durationField;

        private string chargingPolicyCapacityField;

        private int buyingBookingTypeIDField;

        private int buyingPriceTypeIDField;

        private string appliedRuleNamesField;

        private int appliedRuleIDField;

        private MandatoryExtraPrices[] mandatoryExtraPricesField;

        private OptionsMealPlan mealPlanField;

        private OptionDayWiseAllocations[] optionDayWiseAllocationsField;

        private int serviceTypeTypeIdField;

        private decimal priceDiffereneceField;

        private EndPointEssentialInformation essentialInformationField;

        private CancellationPolicyDetail[] clientCancellationPolicyDetailsField;

        private CancellationPolicyDetail[] ePCancellationPolicyDetailsField;

        private TransferInformation transferInformationField;

        private int appliedRulesCountField;

        private int specialOfferRuleCountField;

        private int roomIDField;

        private int optionIDField;

        private string serviceOptionNameField;

        private string optionGroupNameField;

        private string serviceOptionGroupNameField;

        private int quantityField;

        private string rateCodeField;

        private int sequenceNumberField;

        private string freeTextField;

        private string rateCodeNameField;

        private string rateCodeDescriptionField;

        private string optionStatusField;

        private string qty_AvailableField;

        private bool chargingPolicyMileageBasedField;

        private string occupancyTypeIdsField;

        private bool chargingPolicyInsuranceBasedField;

        private bool hasCircuitOfferField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Warning")]
        public PriceAndAvailabilityWarnings[] Warnings
        {
            get
            {
                return this.warningsField;
            }
            set
            {
                this.warningsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int MinAdult
        {
            get
            {
                return this.minAdultField;
            }
            set
            {
                this.minAdultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string TotalGrossSellingPrice
        {
            get
            {
                return this.totalGrossSellingPriceField;
            }
            set
            {
                this.totalGrossSellingPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string TotalSellingPrice
        {
            get
            {
                return this.totalSellingPriceField;
            }
            set
            {
                this.totalSellingPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string TotalTax
        {
            get
            {
                return this.totalTaxField;
            }
            set
            {
                this.totalTaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public AppliedTaxeList APPLIED_TAX
        {
            get
            {
                return this.aPPLIED_TAXField;
            }
            set
            {
                this.aPPLIED_TAXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string TotalBuyingPrice
        {
            get
            {
                return this.totalBuyingPriceField;
            }
            set
            {
                this.totalBuyingPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string EndPointCurrency
        {
            get
            {
                return this.endPointCurrencyField;
            }
            set
            {
                this.endPointCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string TotalEndPointCost
        {
            get
            {
                return this.totalEndPointCostField;
            }
            set
            {
                this.totalEndPointCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public int MaxAdult
        {
            get
            {
                return this.maxAdultField;
            }
            set
            {
                this.maxAdultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public int MinChild
        {
            get
            {
                return this.minChildField;
            }
            set
            {
                this.minChildField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public int MaxChild
        {
            get
            {
                return this.maxChildField;
            }
            set
            {
                this.maxChildField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public int Occupancy
        {
            get
            {
                return this.occupancyField;
            }
            set
            {
                this.occupancyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public OptionOccupancy OptionOccupancy
        {
            get
            {
                return this.optionOccupancyField;
            }
            set
            {
                this.optionOccupancyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 15)]
        public PriceAndAvailabilityResponsePricing[] Prices
        {
            get
            {
                return this.pricesField;
            }
            set
            {
                this.pricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string PriceCode
        {
            get
            {
                return this.priceCodeField;
            }
            set
            {
                this.priceCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string RuleText
        {
            get
            {
                return this.ruleTextField;
            }
            set
            {
                this.ruleTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public double CommissionAmount
        {
            get
            {
                return this.commissionAmountField;
            }
            set
            {
                this.commissionAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 19)]
        public RULE[] APPLIED_RULES
        {
            get
            {
                return this.aPPLIED_RULESField;
            }
            set
            {
                this.aPPLIED_RULESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public int BookingTypeID
        {
            get
            {
                return this.bookingTypeIDField;
            }
            set
            {
                this.bookingTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public int PriceTypeID
        {
            get
            {
                return this.priceTypeIDField;
            }
            set
            {
                this.priceTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public AttachedOptionExtra AttachedOptionExtra
        {
            get
            {
                return this.attachedOptionExtraField;
            }
            set
            {
                this.attachedOptionExtraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string VehicleType
        {
            get
            {
                return this.vehicleTypeField;
            }
            set
            {
                this.vehicleTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public string ChargingPolicyCapacity
        {
            get
            {
                return this.chargingPolicyCapacityField;
            }
            set
            {
                this.chargingPolicyCapacityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public int BuyingBookingTypeID
        {
            get
            {
                return this.buyingBookingTypeIDField;
            }
            set
            {
                this.buyingBookingTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public int BuyingPriceTypeID
        {
            get
            {
                return this.buyingPriceTypeIDField;
            }
            set
            {
                this.buyingPriceTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public string AppliedRuleNames
        {
            get
            {
                return this.appliedRuleNamesField;
            }
            set
            {
                this.appliedRuleNamesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public int AppliedRuleID
        {
            get
            {
                return this.appliedRuleIDField;
            }
            set
            {
                this.appliedRuleIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MandatoryExtraPrices", Order = 30)]
        public MandatoryExtraPrices[] MandatoryExtraPrices
        {
            get
            {
                return this.mandatoryExtraPricesField;
            }
            set
            {
                this.mandatoryExtraPricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 31)]
        public OptionsMealPlan MealPlan
        {
            get
            {
                return this.mealPlanField;
            }
            set
            {
                this.mealPlanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OptionDayWiseAllocations", Order = 32)]
        public OptionDayWiseAllocations[] OptionDayWiseAllocations
        {
            get
            {
                return this.optionDayWiseAllocationsField;
            }
            set
            {
                this.optionDayWiseAllocationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 33)]
        public int ServiceTypeTypeId
        {
            get
            {
                return this.serviceTypeTypeIdField;
            }
            set
            {
                this.serviceTypeTypeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 34)]
        public decimal PriceDifferenece
        {
            get
            {
                return this.priceDiffereneceField;
            }
            set
            {
                this.priceDiffereneceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 35)]
        public EndPointEssentialInformation EssentialInformation
        {
            get
            {
                return this.essentialInformationField;
            }
            set
            {
                this.essentialInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 36)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public CancellationPolicyDetail[] ClientCancellationPolicyDetails
        {
            get
            {
                return this.clientCancellationPolicyDetailsField;
            }
            set
            {
                this.clientCancellationPolicyDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 37)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public CancellationPolicyDetail[] EPCancellationPolicyDetails
        {
            get
            {
                return this.ePCancellationPolicyDetailsField;
            }
            set
            {
                this.ePCancellationPolicyDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 38)]
        public TransferInformation TransferInformation
        {
            get
            {
                return this.transferInformationField;
            }
            set
            {
                this.transferInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 39)]
        public int AppliedRulesCount
        {
            get
            {
                return this.appliedRulesCountField;
            }
            set
            {
                this.appliedRulesCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 40)]
        public int SpecialOfferRuleCount
        {
            get
            {
                return this.specialOfferRuleCountField;
            }
            set
            {
                this.specialOfferRuleCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 41)]
        public int RoomID
        {
            get
            {
                return this.roomIDField;
            }
            set
            {
                this.roomIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 42)]
        public int OptionID
        {
            get
            {
                return this.optionIDField;
            }
            set
            {
                this.optionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 43)]
        public string ServiceOptionName
        {
            get
            {
                return this.serviceOptionNameField;
            }
            set
            {
                this.serviceOptionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 44)]
        public string OptionGroupName
        {
            get
            {
                return this.optionGroupNameField;
            }
            set
            {
                this.optionGroupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 45)]
        public string ServiceOptionGroupName
        {
            get
            {
                return this.serviceOptionGroupNameField;
            }
            set
            {
                this.serviceOptionGroupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 46)]
        public int Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 47)]
        public string RateCode
        {
            get
            {
                return this.rateCodeField;
            }
            set
            {
                this.rateCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 48)]
        public int SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 49)]
        public string FreeText
        {
            get
            {
                return this.freeTextField;
            }
            set
            {
                this.freeTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 50)]
        public string RateCodeName
        {
            get
            {
                return this.rateCodeNameField;
            }
            set
            {
                this.rateCodeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 51)]
        public string RateCodeDescription
        {
            get
            {
                return this.rateCodeDescriptionField;
            }
            set
            {
                this.rateCodeDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 52)]
        public string OptionStatus
        {
            get
            {
                return this.optionStatusField;
            }
            set
            {
                this.optionStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 53)]
        public string Qty_Available
        {
            get
            {
                return this.qty_AvailableField;
            }
            set
            {
                this.qty_AvailableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 54)]
        public bool ChargingPolicyMileageBased
        {
            get
            {
                return this.chargingPolicyMileageBasedField;
            }
            set
            {
                this.chargingPolicyMileageBasedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 55)]
        public string OccupancyTypeIds
        {
            get
            {
                return this.occupancyTypeIdsField;
            }
            set
            {
                this.occupancyTypeIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 56)]
        public bool ChargingPolicyInsuranceBased
        {
            get
            {
                return this.chargingPolicyInsuranceBasedField;
            }
            set
            {
                this.chargingPolicyInsuranceBasedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 57)]
        public bool HasCircuitOffer
        {
            get
            {
                return this.hasCircuitOfferField;
            }
            set
            {
                this.hasCircuitOfferField = value;
            }
        }
    }
    public partial class ServiceTermsDetails
    {

        private int termIDField;

        private PickupDropOffDetails pickupDropOffDetailsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int TermID
        {
            get
            {
                return this.termIDField;
            }
            set
            {
                this.termIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public PickupDropOffDetails PickupDropOffDetails
        {
            get
            {
                return this.pickupDropOffDetailsField;
            }
            set
            {
                this.pickupDropOffDetailsField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityResponseMeals
    {

        private PriceAndAvailabilityResponseBasis basisField;

        private PriceAndAvailabilityResponseBreakfast breakfastField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public PriceAndAvailabilityResponseBasis Basis
        {
            get
            {
                return this.basisField;
            }
            set
            {
                this.basisField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public PriceAndAvailabilityResponseBreakfast Breakfast
        {
            get
            {
                return this.breakfastField;
            }
            set
            {
                this.breakfastField = value;
            }
        }
    }
    public partial class MatchCode
    {

        private string isMainField;

        private string internalCodeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IsMain
        {
            get
            {
                return this.isMainField;
            }
            set
            {
                this.isMainField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string InternalCode
        {
            get
            {
                return this.internalCodeField;
            }
            set
            {
                this.internalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    public partial class ServiceInfo
    {

        private string serviceSourceField;

        private string regionIDField;

        private string regionNameField;

        private string serviceLongNameField;

        private string serviceDescriptionField;

        private string serviceLatituteField;

        private string serviceLongituteField;

        private ServiceImageInfoNew[] serviceImagesField;

        private string serviceRatingField;

        private bool isRecommendedProductField;

        private string address1Field;

        private string address2Field;

        private string address3Field;

        private string cityField;

        private string stateField;

        private string countryField;

        private string phoneNumberField;

        private string faxNumberField;

        private string supplierNameField;

        private string bookingTypeField;

        private string priceTypeField;

        private string endPointNameField;

        private int supplierPreferenceValueField;

        private string serviceSourceIDField;

        private int specialOffersCountField;

        private int searchPriorityField;

        private string priorityNameField;

        private string priorityTextField;

        private byte[] priorityImageField;

        private int priorityImageIdField;

        private int serviceMustStayField;

        private System.Nullable<int> clientCancellationPolicyIDField;

        private System.Nullable<int> supplierCancellationPolicyIDField;

        private string postalCodeField;

        private OperationTime[] operationTimesField;

        private TransferLocationDetails[] transferLocationDetailsField;

        private string serviceRatingsField;

        private string serviceShortNameField;

        private bool serviceStatusNoBookingsField;

        private bool supplierStatusNoBookingsField;

        private bool hasCircuitOfferField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ServiceSource
        {
            get
            {
                return this.serviceSourceField;
            }
            set
            {
                this.serviceSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string RegionID
        {
            get
            {
                return this.regionIDField;
            }
            set
            {
                this.regionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string RegionName
        {
            get
            {
                return this.regionNameField;
            }
            set
            {
                this.regionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ServiceLongName
        {
            get
            {
                return this.serviceLongNameField;
            }
            set
            {
                this.serviceLongNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ServiceDescription
        {
            get
            {
                return this.serviceDescriptionField;
            }
            set
            {
                this.serviceDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string ServiceLatitute
        {
            get
            {
                return this.serviceLatituteField;
            }
            set
            {
                this.serviceLatituteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ServiceLongitute
        {
            get
            {
                return this.serviceLongituteField;
            }
            set
            {
                this.serviceLongituteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ServiceImages", Order = 7)]
        public ServiceImageInfoNew[] ServiceImages
        {
            get
            {
                return this.serviceImagesField;
            }
            set
            {
                this.serviceImagesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string ServiceRating
        {
            get
            {
                return this.serviceRatingField;
            }
            set
            {
                this.serviceRatingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public bool IsRecommendedProduct
        {
            get
            {
                return this.isRecommendedProductField;
            }
            set
            {
                this.isRecommendedProductField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string Address1
        {
            get
            {
                return this.address1Field;
            }
            set
            {
                this.address1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string Address2
        {
            get
            {
                return this.address2Field;
            }
            set
            {
                this.address2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string Address3
        {
            get
            {
                return this.address3Field;
            }
            set
            {
                this.address3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string PhoneNumber
        {
            get
            {
                return this.phoneNumberField;
            }
            set
            {
                this.phoneNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string FaxNumber
        {
            get
            {
                return this.faxNumberField;
            }
            set
            {
                this.faxNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string SupplierName
        {
            get
            {
                return this.supplierNameField;
            }
            set
            {
                this.supplierNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string BookingType
        {
            get
            {
                return this.bookingTypeField;
            }
            set
            {
                this.bookingTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string PriceType
        {
            get
            {
                return this.priceTypeField;
            }
            set
            {
                this.priceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string EndPointName
        {
            get
            {
                return this.endPointNameField;
            }
            set
            {
                this.endPointNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public int SupplierPreferenceValue
        {
            get
            {
                return this.supplierPreferenceValueField;
            }
            set
            {
                this.supplierPreferenceValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string ServiceSourceID
        {
            get
            {
                return this.serviceSourceIDField;
            }
            set
            {
                this.serviceSourceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public int SpecialOffersCount
        {
            get
            {
                return this.specialOffersCountField;
            }
            set
            {
                this.specialOffersCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public int SearchPriority
        {
            get
            {
                return this.searchPriorityField;
            }
            set
            {
                this.searchPriorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public string PriorityName
        {
            get
            {
                return this.priorityNameField;
            }
            set
            {
                this.priorityNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public string PriorityText
        {
            get
            {
                return this.priorityTextField;
            }
            set
            {
                this.priorityTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 28)]
        public byte[] PriorityImage
        {
            get
            {
                return this.priorityImageField;
            }
            set
            {
                this.priorityImageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public int PriorityImageId
        {
            get
            {
                return this.priorityImageIdField;
            }
            set
            {
                this.priorityImageIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 30)]
        public int ServiceMustStay
        {
            get
            {
                return this.serviceMustStayField;
            }
            set
            {
                this.serviceMustStayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 31)]
        public System.Nullable<int> ClientCancellationPolicyID
        {
            get
            {
                return this.clientCancellationPolicyIDField;
            }
            set
            {
                this.clientCancellationPolicyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 32)]
        public System.Nullable<int> SupplierCancellationPolicyID
        {
            get
            {
                return this.supplierCancellationPolicyIDField;
            }
            set
            {
                this.supplierCancellationPolicyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 33)]
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 34)]
        public OperationTime[] OperationTimes
        {
            get
            {
                return this.operationTimesField;
            }
            set
            {
                this.operationTimesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 35)]
        public TransferLocationDetails[] TransferLocationDetails
        {
            get
            {
                return this.transferLocationDetailsField;
            }
            set
            {
                this.transferLocationDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 36)]
        public string ServiceRatings
        {
            get
            {
                return this.serviceRatingsField;
            }
            set
            {
                this.serviceRatingsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 37)]
        public string ServiceShortName
        {
            get
            {
                return this.serviceShortNameField;
            }
            set
            {
                this.serviceShortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 38)]
        public bool ServiceStatusNoBookings
        {
            get
            {
                return this.serviceStatusNoBookingsField;
            }
            set
            {
                this.serviceStatusNoBookingsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 39)]
        public bool SupplierStatusNoBookings
        {
            get
            {
                return this.supplierStatusNoBookingsField;
            }
            set
            {
                this.supplierStatusNoBookingsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 40)]
        public bool HasCircuitOffer
        {
            get
            {
                return this.hasCircuitOfferField;
            }
            set
            {
                this.hasCircuitOfferField = value;
            }
        }
    }
    public partial class Additional_Prices
    {

        private PriceAndAvailabilityResponseServiceOption[] addOptionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public PriceAndAvailabilityResponseServiceOption[] AddOptions
        {
            get
            {
                return this.addOptionsField;
            }
            set
            {
                this.addOptionsField = value;
            }
        }
    }
    public partial class Notes
    {

        private string subjectField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }
    public partial class ServiceTypeRating : BaseType
    {
    }
    public enum enmRoom_Reply
    {

        /// <remarks/>
        ALL_ROOM,

        /// <remarks/>
        ANY_ROOM,

        /// <remarks/>
        ALL_OPTION,
    }
    public partial class RoomDetails
    {

        private int noOfAdultField;

        private int noOfChildrenField;

        private ExtraBed[] extraBedAgesField;

        private int noOfRoomsField;

        private string priceCodeField;

        private string roomTypeField;

        private int optionIdField;

        private ChildNotSharingAges[] childNotSharingAgesField;

        private int occupancyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int NoOfAdult
        {
            get
            {
                return this.noOfAdultField;
            }
            set
            {
                this.noOfAdultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int NoOfChildren
        {
            get
            {
                return this.noOfChildrenField;
            }
            set
            {
                this.noOfChildrenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        public ExtraBed[] ExtraBedAges
        {
            get
            {
                return this.extraBedAgesField;
            }
            set
            {
                this.extraBedAgesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int NoOfRooms
        {
            get
            {
                return this.noOfRoomsField;
            }
            set
            {
                this.noOfRoomsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string PriceCode
        {
            get
            {
                return this.priceCodeField;
            }
            set
            {
                this.priceCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string RoomType
        {
            get
            {
                return this.roomTypeField;
            }
            set
            {
                this.roomTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public int OptionId
        {
            get
            {
                return this.optionIdField;
            }
            set
            {
                this.optionIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        public ChildNotSharingAges[] ChildNotSharingAges
        {
            get
            {
                return this.childNotSharingAgesField;
            }
            set
            {
                this.childNotSharingAgesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public int Occupancy
        {
            get
            {
                return this.occupancyField;
            }
            set
            {
                this.occupancyField = value;
            }
        }
    }
    public partial class InsuranceDetails
    {

        private OptionID[] serviceOptionsField;

        private PaxTypeID[] paxTypesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public OptionID[] ServiceOptions
        {
            get
            {
                return this.serviceOptionsField;
            }
            set
            {
                this.serviceOptionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        public PaxTypeID[] PaxTypes
        {
            get
            {
                return this.paxTypesField;
            }
            set
            {
                this.paxTypesField = value;
            }
        }
    }
    public partial class OptionOccupancy
    {

        private int adultsField;

        private int childrenField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Adults
        {
            get
            {
                return this.adultsField;
            }
            set
            {
                this.adultsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int Children
        {
            get
            {
                return this.childrenField;
            }
            set
            {
                this.childrenField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityResponsePricing
    {

        private string dateField;

        private string sellPriceIdField;

        private string sellPriceField;

        private string buyPriceField;

        private string buyPriceIDField;

        private string currencyCodeField;

        private string chargingPolicyNameField;

        private PriceAndAvailabilityResponseChildPrice[] childPricesField;

        private PriceAndAvailabilityResponseChildPrice[] childNotSharingPricesField;

        private OptionsMealPlan mealPlanField;

        private string costCurrencyCodeField;

        private AppliedTaxeList aPPLIED_TAXField;

        private string originalSellPriceField;

        private AppliedChargingPolicyDetails appliedChargingPolicyDetailsField;

        private string netAmountField;

        private TaxCode[] taxCodesField;

        private bool lATECHECKOUTENABLEDField;

        private decimal lATECHECKOUTCHARGESField;

        private string lATECHECKOUTUNTILField;

        private string tAXField;

        private string gROSSSELLField;

        private string endPointCostField;

        private decimal cOMMISSIONVALUEField;

        private string nETAMOUNTField;

        private string oRIGINALCOSTPRICEAMOUNTField;

        private bool iSPROVISIONALRATEField;

        private double totalInclusiveTaxAppliedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string SellPriceId
        {
            get
            {
                return this.sellPriceIdField;
            }
            set
            {
                this.sellPriceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string SellPrice
        {
            get
            {
                return this.sellPriceField;
            }
            set
            {
                this.sellPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string BuyPrice
        {
            get
            {
                return this.buyPriceField;
            }
            set
            {
                this.buyPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string BuyPriceID
        {
            get
            {
                return this.buyPriceIDField;
            }
            set
            {
                this.buyPriceIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string CurrencyCode
        {
            get
            {
                return this.currencyCodeField;
            }
            set
            {
                this.currencyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ChargingPolicyName
        {
            get
            {
                return this.chargingPolicyNameField;
            }
            set
            {
                this.chargingPolicyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        public PriceAndAvailabilityResponseChildPrice[] ChildPrices
        {
            get
            {
                return this.childPricesField;
            }
            set
            {
                this.childPricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 8)]
        public PriceAndAvailabilityResponseChildPrice[] ChildNotSharingPrices
        {
            get
            {
                return this.childNotSharingPricesField;
            }
            set
            {
                this.childNotSharingPricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public OptionsMealPlan MealPlan
        {
            get
            {
                return this.mealPlanField;
            }
            set
            {
                this.mealPlanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string CostCurrencyCode
        {
            get
            {
                return this.costCurrencyCodeField;
            }
            set
            {
                this.costCurrencyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public AppliedTaxeList APPLIED_TAX
        {
            get
            {
                return this.aPPLIED_TAXField;
            }
            set
            {
                this.aPPLIED_TAXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string OriginalSellPrice
        {
            get
            {
                return this.originalSellPriceField;
            }
            set
            {
                this.originalSellPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public AppliedChargingPolicyDetails AppliedChargingPolicyDetails
        {
            get
            {
                return this.appliedChargingPolicyDetailsField;
            }
            set
            {
                this.appliedChargingPolicyDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string NetAmount
        {
            get
            {
                return this.netAmountField;
            }
            set
            {
                this.netAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TaxCodes", Order = 15)]
        public TaxCode[] TaxCodes
        {
            get
            {
                return this.taxCodesField;
            }
            set
            {
                this.taxCodesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public bool LATECHECKOUTENABLED
        {
            get
            {
                return this.lATECHECKOUTENABLEDField;
            }
            set
            {
                this.lATECHECKOUTENABLEDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public decimal LATECHECKOUTCHARGES
        {
            get
            {
                return this.lATECHECKOUTCHARGESField;
            }
            set
            {
                this.lATECHECKOUTCHARGESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string LATECHECKOUTUNTIL
        {
            get
            {
                return this.lATECHECKOUTUNTILField;
            }
            set
            {
                this.lATECHECKOUTUNTILField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public string TAX
        {
            get
            {
                return this.tAXField;
            }
            set
            {
                this.tAXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string GROSSSELL
        {
            get
            {
                return this.gROSSSELLField;
            }
            set
            {
                this.gROSSSELLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string EndPointCost
        {
            get
            {
                return this.endPointCostField;
            }
            set
            {
                this.endPointCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public decimal COMMISSIONVALUE
        {
            get
            {
                return this.cOMMISSIONVALUEField;
            }
            set
            {
                this.cOMMISSIONVALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string NETAMOUNT1
        {
            get
            {
                return this.nETAMOUNTField;
            }
            set
            {
                this.nETAMOUNTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string ORIGINALCOSTPRICEAMOUNT
        {
            get
            {
                return this.oRIGINALCOSTPRICEAMOUNTField;
            }
            set
            {
                this.oRIGINALCOSTPRICEAMOUNTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public bool ISPROVISIONALRATE
        {
            get
            {
                return this.iSPROVISIONALRATEField;
            }
            set
            {
                this.iSPROVISIONALRATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public double TotalInclusiveTaxApplied
        {
            get
            {
                return this.totalInclusiveTaxAppliedField;
            }
            set
            {
                this.totalInclusiveTaxAppliedField = value;
            }
        }
    }
    public partial class RULE
    {

        private string rULE_NAMEField;

        private int rULE_IDField;

        private string aPPLIED_RULE_IDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RULE_NAME
        {
            get
            {
                return this.rULE_NAMEField;
            }
            set
            {
                this.rULE_NAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int RULE_ID
        {
            get
            {
                return this.rULE_IDField;
            }
            set
            {
                this.rULE_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string APPLIED_RULE_ID
        {
            get
            {
                return this.aPPLIED_RULE_IDField;
            }
            set
            {
                this.aPPLIED_RULE_IDField = value;
            }
        }
    }
    public partial class AttachedOptionExtra
    {

        private option[] attachedOptionsField;

        private int[] attachedExtrasField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("BookServiceResponse")]
        public option[] AttachedOptions
        {
            get
            {
                return this.attachedOptionsField;
            }
            set
            {
                this.attachedOptionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        public int[] AttachedExtras
        {
            get
            {
                return this.attachedExtrasField;
            }
            set
            {
                this.attachedExtrasField = value;
            }
        }
    }
    public partial class MandatoryExtraPrices
    {

        private int extraIDField;

        private decimal totalAmountField;

        private decimal taxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int ExtraID
        {
            get
            {
                return this.extraIDField;
            }
            set
            {
                this.extraIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public decimal TotalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public decimal Tax
        {
            get
            {
                return this.taxField;
            }
            set
            {
                this.taxField = value;
            }
        }
    }
    public partial class OptionsMealPlan
    {

        private int mealPlanIDField;

        private string mealPlanNameField;

        private string mealPlanCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int MealPlanID
        {
            get
            {
                return this.mealPlanIDField;
            }
            set
            {
                this.mealPlanIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string MealPlanName
        {
            get
            {
                return this.mealPlanNameField;
            }
            set
            {
                this.mealPlanNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MealPlanCode
        {
            get
            {
                return this.mealPlanCodeField;
            }
            set
            {
                this.mealPlanCodeField = value;
            }
        }
    }
    public partial class OptionDayWiseAllocations
    {

        private int dayField;

        private string dATEField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string DATE
        {
            get
            {
                return this.dATEField;
            }
            set
            {
                this.dATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }
    public partial class EndPointEssentialInformation
    {

        private EndPointInformation[] informationField;

        private enmBedSharing sharingBedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Information", Order = 0)]
        public EndPointInformation[] Information
        {
            get
            {
                return this.informationField;
            }
            set
            {
                this.informationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public enmBedSharing SharingBed
        {
            get
            {
                return this.sharingBedField;
            }
            set
            {
                this.sharingBedField = value;
            }
        }
    }
    public partial class CancellationPolicyDetail
    {

        private string daysField;

        private string chargeAmountField;

        private string chargeTypeField;

        private string chargeCurrencyField;

        private System.Nullable<System.DateTime> cancellationDateField;

        private int chargeOfField;

        private string costAmountField;

        private string costCurrencyField;

        private TravelDateRange travelDateRangeField;

        private BookingCreationDateRange bookingCreationDateRangeField;

        private string descriptionField;

        private bool onlyDescriptionField;

        private string clientCancellationPolicyField;

        private System.Nullable<int> bookingAppliedCancellationPolicyIDField;

        private System.Nullable<int> cancellationPolicyIdField;

        private string cancellationPolicyNameField;

        private System.Nullable<int> serviceIdField;

        private string serviceNameField;

        private System.Nullable<int> bookedServiceIdField;

        private System.Nullable<int> bookedPackageIdField;

        private System.Nullable<int> bookedOptionIdField;

        private string bookedOptionInDateField;

        private double cancellationAmountField;

        private string passengerNameField;

        private string optionElementNameField;

        private bool isVendorPolicyField;

        private double valueOfOtherServicesField;

        private double totalChargeField;

        private double totalBalanceField;

        private double totalInvoiceField;

        private bool generateCancellationMessageField;

        private System.Nullable<int> packageDepartureIdField;

        private string calculateUsingDescriptionField;

        private string applicationDescriptionField;

        private string calculateUsingIDField;

        private string applicationIDField;

        private string bookedOptionCostCurrencyISOCodeField;

        private string bookedOptionSellCurrencyISOCodeField;

        private System.Nullable<bool> isReliableEndpointField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Days
        {
            get
            {
                return this.daysField;
            }
            set
            {
                this.daysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ChargeAmount
        {
            get
            {
                return this.chargeAmountField;
            }
            set
            {
                this.chargeAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ChargeType
        {
            get
            {
                return this.chargeTypeField;
            }
            set
            {
                this.chargeTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ChargeCurrency
        {
            get
            {
                return this.chargeCurrencyField;
            }
            set
            {
                this.chargeCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public System.Nullable<System.DateTime> CancellationDate
        {
            get
            {
                return this.cancellationDateField;
            }
            set
            {
                this.cancellationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public int ChargeOf
        {
            get
            {
                return this.chargeOfField;
            }
            set
            {
                this.chargeOfField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string CostAmount
        {
            get
            {
                return this.costAmountField;
            }
            set
            {
                this.costAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string CostCurrency
        {
            get
            {
                return this.costCurrencyField;
            }
            set
            {
                this.costCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public TravelDateRange TravelDateRange
        {
            get
            {
                return this.travelDateRangeField;
            }
            set
            {
                this.travelDateRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public BookingCreationDateRange BookingCreationDateRange
        {
            get
            {
                return this.bookingCreationDateRangeField;
            }
            set
            {
                this.bookingCreationDateRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool OnlyDescription
        {
            get
            {
                return this.onlyDescriptionField;
            }
            set
            {
                this.onlyDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string ClientCancellationPolicy
        {
            get
            {
                return this.clientCancellationPolicyField;
            }
            set
            {
                this.clientCancellationPolicyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 13)]
        public System.Nullable<int> BookingAppliedCancellationPolicyID
        {
            get
            {
                return this.bookingAppliedCancellationPolicyIDField;
            }
            set
            {
                this.bookingAppliedCancellationPolicyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 14)]
        public System.Nullable<int> CancellationPolicyId
        {
            get
            {
                return this.cancellationPolicyIdField;
            }
            set
            {
                this.cancellationPolicyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string CancellationPolicyName
        {
            get
            {
                return this.cancellationPolicyNameField;
            }
            set
            {
                this.cancellationPolicyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 16)]
        public System.Nullable<int> ServiceId
        {
            get
            {
                return this.serviceIdField;
            }
            set
            {
                this.serviceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public string ServiceName
        {
            get
            {
                return this.serviceNameField;
            }
            set
            {
                this.serviceNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 18)]
        public System.Nullable<int> BookedServiceId
        {
            get
            {
                return this.bookedServiceIdField;
            }
            set
            {
                this.bookedServiceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 19)]
        public System.Nullable<int> BookedPackageId
        {
            get
            {
                return this.bookedPackageIdField;
            }
            set
            {
                this.bookedPackageIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 20)]
        public System.Nullable<int> BookedOptionId
        {
            get
            {
                return this.bookedOptionIdField;
            }
            set
            {
                this.bookedOptionIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public string BookedOptionInDate
        {
            get
            {
                return this.bookedOptionInDateField;
            }
            set
            {
                this.bookedOptionInDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public double CancellationAmount
        {
            get
            {
                return this.cancellationAmountField;
            }
            set
            {
                this.cancellationAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public string PassengerName
        {
            get
            {
                return this.passengerNameField;
            }
            set
            {
                this.passengerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public string OptionElementName
        {
            get
            {
                return this.optionElementNameField;
            }
            set
            {
                this.optionElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public bool IsVendorPolicy
        {
            get
            {
                return this.isVendorPolicyField;
            }
            set
            {
                this.isVendorPolicyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public double ValueOfOtherServices
        {
            get
            {
                return this.valueOfOtherServicesField;
            }
            set
            {
                this.valueOfOtherServicesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public double TotalCharge
        {
            get
            {
                return this.totalChargeField;
            }
            set
            {
                this.totalChargeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public double TotalBalance
        {
            get
            {
                return this.totalBalanceField;
            }
            set
            {
                this.totalBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public double TotalInvoice
        {
            get
            {
                return this.totalInvoiceField;
            }
            set
            {
                this.totalInvoiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 30)]
        public bool GenerateCancellationMessage
        {
            get
            {
                return this.generateCancellationMessageField;
            }
            set
            {
                this.generateCancellationMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 31)]
        public System.Nullable<int> PackageDepartureId
        {
            get
            {
                return this.packageDepartureIdField;
            }
            set
            {
                this.packageDepartureIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 32)]
        public string CalculateUsingDescription
        {
            get
            {
                return this.calculateUsingDescriptionField;
            }
            set
            {
                this.calculateUsingDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 33)]
        public string ApplicationDescription
        {
            get
            {
                return this.applicationDescriptionField;
            }
            set
            {
                this.applicationDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 34)]
        public string CalculateUsingID
        {
            get
            {
                return this.calculateUsingIDField;
            }
            set
            {
                this.calculateUsingIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 35)]
        public string ApplicationID
        {
            get
            {
                return this.applicationIDField;
            }
            set
            {
                this.applicationIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 36)]
        public string BookedOptionCostCurrencyISOCode
        {
            get
            {
                return this.bookedOptionCostCurrencyISOCodeField;
            }
            set
            {
                this.bookedOptionCostCurrencyISOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 37)]
        public string BookedOptionSellCurrencyISOCode
        {
            get
            {
                return this.bookedOptionSellCurrencyISOCodeField;
            }
            set
            {
                this.bookedOptionSellCurrencyISOCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 38)]
        public System.Nullable<bool> IsReliableEndpoint
        {
            get
            {
                return this.isReliableEndpointField;
            }
            set
            {
                this.isReliableEndpointField = value;
            }
        }
    }

    public partial class TransferInformation
    {

        private int passengerCapacityField;

        private int luggageCapacityLargeField;

        private int luggageCapacitySmallField;

        private string acrissCodeField;

        private VehicalImage[] imagesField;

        private string vehicleTypeField;

        private DescriptionList[] descriptionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int PassengerCapacity
        {
            get
            {
                return this.passengerCapacityField;
            }
            set
            {
                this.passengerCapacityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int LuggageCapacityLarge
        {
            get
            {
                return this.luggageCapacityLargeField;
            }
            set
            {
                this.luggageCapacityLargeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int LuggageCapacitySmall
        {
            get
            {
                return this.luggageCapacitySmallField;
            }
            set
            {
                this.luggageCapacitySmallField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string AcrissCode
        {
            get
            {
                return this.acrissCodeField;
            }
            set
            {
                this.acrissCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Image")]
        public VehicalImage[] Images
        {
            get
            {
                return this.imagesField;
            }
            set
            {
                this.imagesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string VehicleType
        {
            get
            {
                return this.vehicleTypeField;
            }
            set
            {
                this.vehicleTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Description")]
        public DescriptionList[] Descriptions
        {
            get
            {
                return this.descriptionsField;
            }
            set
            {
                this.descriptionsField = value;
            }
        }
    }
   public partial class PickupDropOffDetails
    {
        
        private int regionIDField;
        
        private string regionNameField;
        
        private enmBookingTranferTypeRQ locationTypeField;
        
        private int locationIDField;
        
        private string locationCodeField;
        
        private string noOfHoursField;
        
        private PickupDetails pickupDetailsField;
        
        private DropoffDetails dropoffDetailsField;
        
        private LinkedServicesData[] linkedServicesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int RegionID
        {
            get
            {
                return this.regionIDField;
            }
            set
            {
                this.regionIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string RegionName
        {
            get
            {
                return this.regionNameField;
            }
            set
            {
                this.regionNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public enmBookingTranferTypeRQ LocationType
        {
            get
            {
                return this.locationTypeField;
            }
            set
            {
                this.locationTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int LocationID
        {
            get
            {
                return this.locationIDField;
            }
            set
            {
                this.locationIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string LocationCode
        {
            get
            {
                return this.locationCodeField;
            }
            set
            {
                this.locationCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string NoOfHours
        {
            get
            {
                return this.noOfHoursField;
            }
            set
            {
                this.noOfHoursField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public PickupDetails PickupDetails
        {
            get
            {
                return this.pickupDetailsField;
            }
            set
            {
                this.pickupDetailsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public DropoffDetails DropoffDetails
        {
            get
            {
                return this.dropoffDetailsField;
            }
            set
            {
                this.dropoffDetailsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=8)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Service", IsNullable=false)]
        public LinkedServicesData[] LinkedServices
        {
            get
            {
                return this.linkedServicesField;
            }
            set
            {
                this.linkedServicesField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityResponseBasis
    {

        private string codeField;

        private string basisNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string basisName
        {
            get
            {
                return this.basisNameField;
            }
            set
            {
                this.basisNameField = value;
            }
        }
    }
     public partial class DropoffDetails
    {
        
        private string dropoffTimeField;
        
        private string dropoffDescriptionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string DropoffTime
        {
            get
            {
                return this.dropoffTimeField;
            }
            set
            {
                this.dropoffTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string DropoffDescription
        {
            get
            {
                return this.dropoffDescriptionField;
            }
            set
            {
                this.dropoffDescriptionField = value;
            }
        }
    }
    public partial class PickUpDetail
    {

        private string timeField;

        private string descriptionField;

        private int packageTermsIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int PackageTermsID
        {
            get
            {
                return this.packageTermsIDField;
            }
            set
            {
                this.packageTermsIDField = value;
            }
        }
    }
    public partial class PickupDetails
    {

        private string pickupTimeField;

        private string pickupDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string PickupTime
        {
            get
            {
                return this.pickupTimeField;
            }
            set
            {
                this.pickupTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string PickupDescription
        {
            get
            {
                return this.pickupDescriptionField;
            }
            set
            {
                this.pickupDescriptionField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityResponseBreakfast
    {

        private string codeField;

        private string bFNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string BFName
        {
            get
            {
                return this.bFNameField;
            }
            set
            {
                this.bFNameField = value;
            }
        }
    }
    public partial class ServiceImageInfoNew
    {

        private ServiceImageSingle imageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ServiceImageSingle Image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                this.imageField = value;
            }
        }
    }
    public partial class OperationTime
    {

        private string endField;

        private string startField;

        private WeekDay weekDayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string End
        {
            get
            {
                return this.endField;
            }
            set
            {
                this.endField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public WeekDay WeekDay
        {
            get
            {
                return this.weekDayField;
            }
            set
            {
                this.weekDayField = value;
            }
        }
    }
    public partial class ExtraBed
    {

        private int roomField;

        private int ageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Room
        {
            get
            {
                return this.roomField;
            }
            set
            {
                this.roomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int Age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }
    }
    public enum WeekDay
    {

        /// <remarks/>
        Sun,

        /// <remarks/>
        Mon,

        /// <remarks/>
        Tue,

        /// <remarks/>
        Wed,

        /// <remarks/>
        Thu,

        /// <remarks/>
        Fri,

        /// <remarks/>
        Sat,
    }
    public partial class ServiceImageSingle
    {

        private string imageUrlField;

        private byte[] imageDataField;

        private int serviceImageIDField;

        private string serviceImageNameField;

        private string serviceImagePicField;

        private string serviceImageTypeField;

        private string imageAttributionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ImageUrl
        {
            get
            {
                return this.imageUrlField;
            }
            set
            {
                this.imageUrlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 1)]
        public byte[] ImageData
        {
            get
            {
                return this.imageDataField;
            }
            set
            {
                this.imageDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int ServiceImageID
        {
            get
            {
                return this.serviceImageIDField;
            }
            set
            {
                this.serviceImageIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ServiceImageName
        {
            get
            {
                return this.serviceImageNameField;
            }
            set
            {
                this.serviceImageNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ServiceImagePic
        {
            get
            {
                return this.serviceImagePicField;
            }
            set
            {
                this.serviceImagePicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string ServiceImageType
        {
            get
            {
                return this.serviceImageTypeField;
            }
            set
            {
                this.serviceImageTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ImageAttribution
        {
            get
            {
                return this.imageAttributionField;
            }
            set
            {
                this.imageAttributionField = value;
            }
        }
    }
    public partial class BookingCreationDateRange
    {

        private System.Nullable<System.DateTime> fromDateField;

        private System.Nullable<System.DateTime> toDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public System.Nullable<System.DateTime> FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public System.Nullable<System.DateTime> ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }
    public partial class TravelDateRange
    {

        private System.Nullable<System.DateTime> fromDateField;

        private System.Nullable<System.DateTime> toDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public System.Nullable<System.DateTime> FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public System.Nullable<System.DateTime> ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }
    public partial class EndPointInformation
    {

        private string textField;

        private DateRange dateRangeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DateRange DateRange
        {
            get
            {
                return this.dateRangeField;
            }
            set
            {
                this.dateRangeField = value;
            }
        }
    }
    public partial class DateRange
    {

        private string fromDateField;

        private string toDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string FromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ToDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }
    }
    public partial class DescriptionList
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    public partial class ChildNotSharingAges
    {

        private int ageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }
    }
    public partial class TaxCode
    {

        private int idField;

        private string nameField;

        private string descriptionField;

        private int taxValueField;

        private decimal rateField;

        private bool taxIncludedField;

        private string shortNameField;

        private System.Nullable<int> regionIDField;

        private string regionNameField;

        private TaxValue[] taxValuesField;

        private enmApplyTo applyToField;

        private AssignedServiceType[] serviceTypesField;

        private bool isActiveField;

        private bool isDefaultForFastBuildField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int TaxValue
        {
            get
            {
                return this.taxValueField;
            }
            set
            {
                this.taxValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public decimal Rate
        {
            get
            {
                return this.rateField;
            }
            set
            {
                this.rateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public bool TaxIncluded
        {
            get
            {
                return this.taxIncludedField;
            }
            set
            {
                this.taxIncludedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 7)]
        public System.Nullable<int> RegionID
        {
            get
            {
                return this.regionIDField;
            }
            set
            {
                this.regionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string RegionName
        {
            get
            {
                return this.regionNameField;
            }
            set
            {
                this.regionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 9)]
        public TaxValue[] TaxValues
        {
            get
            {
                return this.taxValuesField;
            }
            set
            {
                this.taxValuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public enmApplyTo ApplyTo
        {
            get
            {
                return this.applyToField;
            }
            set
            {
                this.applyToField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ServiceType")]
        public AssignedServiceType[] ServiceTypes
        {
            get
            {
                return this.serviceTypesField;
            }
            set
            {
                this.serviceTypesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool IsActive
        {
            get
            {
                return this.isActiveField;
            }
            set
            {
                this.isActiveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public bool IsDefaultForFastBuild
        {
            get
            {
                return this.isDefaultForFastBuildField;
            }
            set
            {
                this.isDefaultForFastBuildField = value;
            }
        }
    }
    public partial class PaxTypeID
    {

        private int countField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    public partial class AssignedServiceType
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }
    public enum enmApplyTo
    {

        /// <remarks/>
        All,

        /// <remarks/>
        Client,

        /// <remarks/>
        Supplier,
    }
    public enum enmBedSharing
    {

        /// <remarks/>
        BedSharingNotProvided,

        /// <remarks/>
        SharingBed,

        /// <remarks/>
        NonSharingBed,
    }
    public partial class OptionID
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public int Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    public partial class PriceAndAvailabilityResponseChildPrice
    {

        private string ageField;

        private double priceAmountField;

        private AppliedTaxeList aPPLIED_TAXField;

        private double buyingPriceAmountField;

        private int childQuantityField;

        private string netAmountField;

        private string childOriginalCostPriceAmountField;

        private string childOriginalSellPriceAmountField;

        private decimal cHILD_COMMISSIONVALUEField;

        private string cHILD_NETAMOUNTField;

        private string totalInclusiveTaxAppliedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public double PriceAmount
        {
            get
            {
                return this.priceAmountField;
            }
            set
            {
                this.priceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public AppliedTaxeList APPLIED_TAX
        {
            get
            {
                return this.aPPLIED_TAXField;
            }
            set
            {
                this.aPPLIED_TAXField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public double BuyingPriceAmount
        {
            get
            {
                return this.buyingPriceAmountField;
            }
            set
            {
                this.buyingPriceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public int ChildQuantity
        {
            get
            {
                return this.childQuantityField;
            }
            set
            {
                this.childQuantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string NetAmount
        {
            get
            {
                return this.netAmountField;
            }
            set
            {
                this.netAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ChildOriginalCostPriceAmount
        {
            get
            {
                return this.childOriginalCostPriceAmountField;
            }
            set
            {
                this.childOriginalCostPriceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string ChildOriginalSellPriceAmount
        {
            get
            {
                return this.childOriginalSellPriceAmountField;
            }
            set
            {
                this.childOriginalSellPriceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public decimal CHILD_COMMISSIONVALUE
        {
            get
            {
                return this.cHILD_COMMISSIONVALUEField;
            }
            set
            {
                this.cHILD_COMMISSIONVALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string CHILD_NETAMOUNT
        {
            get
            {
                return this.cHILD_NETAMOUNTField;
            }
            set
            {
                this.cHILD_NETAMOUNTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string TotalInclusiveTaxApplied
        {
            get
            {
                return this.totalInclusiveTaxAppliedField;
            }
            set
            {
                this.totalInclusiveTaxAppliedField = value;
            }
        }
    }
    public partial class TransferLocationDetails
    {

        private string codeField;

        private string nameField;

        private string dateField;

        private string timeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }
    }
    public partial class TaxValue
    {

        private int valueIdField;

        private System.DateTime startDateField;

        private System.DateTime endDateField;

        private bool fixedField;

        private float rateField;

        private double amountField;

        private bool cumulativeField;

        private char beforeMarkUpField;

        private bool netRateInvoiceField;

        private bool inclusiveField;

        private bool applyOnlyToMarkUpField;

        private bool perPersonPerNightField;

        private bool includeOnNetField;

        private bool baseCostTaxField;

        private bool useForQuoteCostField;

        private bool perPersonField;

        private bool calculateOnCostField;

        private bool showingBookingField;

        private bool addToBuySellField;

        private bool perTotalField;

        private bool perRoomField;

        private bool perRoomPerNightField;

        private bool afterCommissionField;

        private bool basedOnNetValueField;

        private bool calculateAsExclusiveTaxField;

        private System.Nullable<int> gLCostAccountIDField;

        private System.Nullable<int> gLRevenueAccountIDField;

        private bool defaultForFastBuildField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int ValueId
        {
            get
            {
                return this.valueIdField;
            }
            set
            {
                this.valueIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool Fixed
        {
            get
            {
                return this.fixedField;
            }
            set
            {
                this.fixedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public float Rate
        {
            get
            {
                return this.rateField;
            }
            set
            {
                this.rateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public double Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public bool Cumulative
        {
            get
            {
                return this.cumulativeField;
            }
            set
            {
                this.cumulativeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public char BeforeMarkUp
        {
            get
            {
                return this.beforeMarkUpField;
            }
            set
            {
                this.beforeMarkUpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public bool NetRateInvoice
        {
            get
            {
                return this.netRateInvoiceField;
            }
            set
            {
                this.netRateInvoiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public bool Inclusive
        {
            get
            {
                return this.inclusiveField;
            }
            set
            {
                this.inclusiveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public bool ApplyOnlyToMarkUp
        {
            get
            {
                return this.applyOnlyToMarkUpField;
            }
            set
            {
                this.applyOnlyToMarkUpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool PerPersonPerNight
        {
            get
            {
                return this.perPersonPerNightField;
            }
            set
            {
                this.perPersonPerNightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool IncludeOnNet
        {
            get
            {
                return this.includeOnNetField;
            }
            set
            {
                this.includeOnNetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public bool BaseCostTax
        {
            get
            {
                return this.baseCostTaxField;
            }
            set
            {
                this.baseCostTaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public bool UseForQuoteCost
        {
            get
            {
                return this.useForQuoteCostField;
            }
            set
            {
                this.useForQuoteCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public bool PerPerson
        {
            get
            {
                return this.perPersonField;
            }
            set
            {
                this.perPersonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public bool CalculateOnCost
        {
            get
            {
                return this.calculateOnCostField;
            }
            set
            {
                this.calculateOnCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public bool ShowingBooking
        {
            get
            {
                return this.showingBookingField;
            }
            set
            {
                this.showingBookingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public bool AddToBuySell
        {
            get
            {
                return this.addToBuySellField;
            }
            set
            {
                this.addToBuySellField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public bool PerTotal
        {
            get
            {
                return this.perTotalField;
            }
            set
            {
                this.perTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public bool PerRoom
        {
            get
            {
                return this.perRoomField;
            }
            set
            {
                this.perRoomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public bool PerRoomPerNight
        {
            get
            {
                return this.perRoomPerNightField;
            }
            set
            {
                this.perRoomPerNightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public bool AfterCommission
        {
            get
            {
                return this.afterCommissionField;
            }
            set
            {
                this.afterCommissionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public bool BasedOnNetValue
        {
            get
            {
                return this.basedOnNetValueField;
            }
            set
            {
                this.basedOnNetValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public bool CalculateAsExclusiveTax
        {
            get
            {
                return this.calculateAsExclusiveTaxField;
            }
            set
            {
                this.calculateAsExclusiveTaxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 25)]
        public System.Nullable<int> GLCostAccountID
        {
            get
            {
                return this.gLCostAccountIDField;
            }
            set
            {
                this.gLCostAccountIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 26)]
        public System.Nullable<int> GLRevenueAccountID
        {
            get
            {
                return this.gLRevenueAccountIDField;
            }
            set
            {
                this.gLRevenueAccountIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public bool DefaultForFastBuild
        {
            get
            {
                return this.defaultForFastBuildField;
            }
            set
            {
                this.defaultForFastBuildField = value;
            }
        }
    }
    public partial class option
    {

        private int optionIDField;

        private string optionNameField;

        private OccupancyType[] occupancyTypesField;

        private Allocation[] allocationsField;

        private string optionDateField;

        private string optionOutDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int OptionID
        {
            get
            {
                return this.optionIDField;
            }
            set
            {
                this.optionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string OptionName
        {
            get
            {
                return this.optionNameField;
            }
            set
            {
                this.optionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        public OccupancyType[] OccupancyTypes
        {
            get
            {
                return this.occupancyTypesField;
            }
            set
            {
                this.occupancyTypesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        public Allocation[] Allocations
        {
            get
            {
                return this.allocationsField;
            }
            set
            {
                this.allocationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string OptionDate
        {
            get
            {
                return this.optionDateField;
            }
            set
            {
                this.optionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string OptionOutDate
        {
            get
            {
                return this.optionOutDateField;
            }
            set
            {
                this.optionOutDateField = value;
            }
        }
    }
    public partial class VehicalImage
    {

        private string uRLField;

        private string imageTypeField;

        private int imageIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string URL
        {
            get
            {
                return this.uRLField;
            }
            set
            {
                this.uRLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ImageType
        {
            get
            {
                return this.imageTypeField;
            }
            set
            {
                this.imageTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int ImageID
        {
            get
            {
                return this.imageIDField;
            }
            set
            {
                this.imageIDField = value;
            }
        }
    }
    public partial class Allocation
    {

        private string allocationNameField;

        private int allocationIDField;

        private ServiceDuration[] serviceDurationsField;

        private string currencyField;

        private ServicePrice[] servicePricesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AllocationName
        {
            get
            {
                return this.allocationNameField;
            }
            set
            {
                this.allocationNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int AllocationID
        {
            get
            {
                return this.allocationIDField;
            }
            set
            {
                this.allocationIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        public ServiceDuration[] ServiceDurations
        {
            get
            {
                return this.serviceDurationsField;
            }
            set
            {
                this.serviceDurationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        public ServicePrice[] ServicePrices
        {
            get
            {
                return this.servicePricesField;
            }
            set
            {
                this.servicePricesField = value;
            }
        }
    }
    public partial class ServicePrice
    {

        private int bookingTypeField;

        private string bookingTypeNameField;

        private int priceTypeField;

        private string priceTypeNameField;

        private string sellPriceField;

        private string priceFromDateField;

        private string priceToDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int BookingType
        {
            get
            {
                return this.bookingTypeField;
            }
            set
            {
                this.bookingTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string BookingTypeName
        {
            get
            {
                return this.bookingTypeNameField;
            }
            set
            {
                this.bookingTypeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int PriceType
        {
            get
            {
                return this.priceTypeField;
            }
            set
            {
                this.priceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string PriceTypeName
        {
            get
            {
                return this.priceTypeNameField;
            }
            set
            {
                this.priceTypeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string SellPrice
        {
            get
            {
                return this.sellPriceField;
            }
            set
            {
                this.sellPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string PriceFromDate
        {
            get
            {
                return this.priceFromDateField;
            }
            set
            {
                this.priceFromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string PriceToDate
        {
            get
            {
                return this.priceToDateField;
            }
            set
            {
                this.priceToDateField = value;
            }
        }
    }
    public partial class ServiceDuration
    {

        private string serviceFromDateField;

        private string serviceToDateField;

        private int perDayQtyAvailableField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ServiceFromDate
        {
            get
            {
                return this.serviceFromDateField;
            }
            set
            {
                this.serviceFromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ServiceToDate
        {
            get
            {
                return this.serviceToDateField;
            }
            set
            {
                this.serviceToDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int PerDayQtyAvailable
        {
            get
            {
                return this.perDayQtyAvailableField;
            }
            set
            {
                this.perDayQtyAvailableField = value;
            }
        }
    }
    public partial class OccupancyType
    {

        private int idField;

        private string nameField;

        private int occupancyTypeCapacityField;

        private int childCapacityField;

        private string aliasField;

        private bool activeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int OccupancyTypeCapacity
        {
            get
            {
                return this.occupancyTypeCapacityField;
            }
            set
            {
                this.occupancyTypeCapacityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int ChildCapacity
        {
            get
            {
                return this.childCapacityField;
            }
            set
            {
                this.childCapacityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Alias
        {
            get
            {
                return this.aliasField;
            }
            set
            {
                this.aliasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public bool Active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
            }
        }
    }
     public partial class LinkedServicesData
    {
        
        private int idField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }
     public enum enmBookingTranferTypeRQ
    {
        
        /// <remarks/>
        Airport,
        
        /// <remarks/>
        Station,
        
        /// <remarks/>
        Port,
        
        /// <remarks/>
        Hotel,
        
        /// <remarks/>
        Location,
    }
    
}
