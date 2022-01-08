using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.TSv2ApiEntities
{
    public class GetServicePricesRequest : BaseRequest
    {               
        private RequestResponseError[] errorsField;
        
        private UIPricesAndAvailabilityRequest[] arrayOfUIPricesAndAvailabilityRequestField;
        
        private string applyExchangeRatesField;
        
        private string countryCodeForPassengerTaxField;
        
        private int cLIENTIDField;
        
        private int bOOKING_TYPE_IDField;
        
        private int pRICE_TYPE_IDField;
        
        private enmReturnPrices returnPricesField;
        
        private int processingModeField;
        
        private string isServiceOptionDescriptionRequiredField;
        
        private string calculateCommissonField;
        
        private string calculateGSTTaxField;
        
        private string isMeanPlanDetailsRequiredField;
        
        private string returnMatchCodeField;
        
        private bool gEO_LOCATION_TREE_REQUIREDField;
        
        private string gEO_LOCATION_NAMEField;
        
        private string gEO_LOCATION_IDField;
        
        private bool notesRequiredField;
        
        private enmBestSeller bestSellerField;
        
        private bool bestSellerFieldSpecified;
        
        private bool returnAppliedChargingPolicyDetailsField;
        
        private int iMAGENOTREQUIREDField;
        
        private string bookingRefNoField;
        
        private System.Nullable<int> nationalityIDField;
        
        private bool returnAttachedOptionExtraField;
        
        private bool rETURNCHEAPESTPRICEField;
        
        private string useMultiplePricesByBookingAndPriceTypeField;
        
        private int leadPassengerCountryIDField;
        
        private UIPricesAndAvailabilitiesRequestModifiers modifiersField;
        
        private ServiceSearchType serviceSearchTypeField;
        
        private bool returnMandatoryExtraPricesField;
        
        private bool isEstimatedServiceField;
        
        private int isPackageServiceField;
        
        private bool applyBTPTFilterField;
        
        private EstimatedServiceRatesInfo estimatedServiceRatesInfoField;
        
        private RulesValidationModifier validateRulesModifierField;
        
        private bool calledFromFITField;
        
        private System.Nullable<int> proposalEnquiryIdField;
        
        private bool isLinkedServiceField;
        public int OrganisationId;
        public Guid SiteId;

        public int AdultCount { get; set; }
        
        public GetServicePricesRequest()
        {
            this.returnMandatoryExtraPricesField = false;
            this.isEstimatedServiceField = false;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=0)]
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
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ApplyExchangeRates
        {
            get
            {
                return this.applyExchangeRatesField;
            }
            set
            {
                this.applyExchangeRatesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string CountryCodeForPassengerTax
        {
            get
            {
                return this.countryCodeForPassengerTaxField;
            }
            set
            {
                this.countryCodeForPassengerTaxField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int CLIENTID
        {
            get
            {
                return this.cLIENTIDField;
            }
            set
            {
                this.cLIENTIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int BOOKING_TYPE_ID
        {
            get
            {
                return this.bOOKING_TYPE_IDField;
            }
            set
            {
                this.bOOKING_TYPE_IDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public int PRICE_TYPE_ID
        {
            get
            {
                return this.pRICE_TYPE_IDField;
            }
            set
            {
                this.pRICE_TYPE_IDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public enmReturnPrices ReturnPrices
        {
            get
            {
                return this.returnPricesField;
            }
            set
            {
                this.returnPricesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public int ProcessingMode
        {
            get
            {
                return this.processingModeField;
            }
            set
            {
                this.processingModeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string IsServiceOptionDescriptionRequired
        {
            get
            {
                return this.isServiceOptionDescriptionRequiredField;
            }
            set
            {
                this.isServiceOptionDescriptionRequiredField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string CalculateCommisson
        {
            get
            {
                return this.calculateCommissonField;
            }
            set
            {
                this.calculateCommissonField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string CalculateGSTTax
        {
            get
            {
                return this.calculateGSTTaxField;
            }
            set
            {
                this.calculateGSTTaxField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string IsMeanPlanDetailsRequired
        {
            get
            {
                return this.isMeanPlanDetailsRequiredField;
            }
            set
            {
                this.isMeanPlanDetailsRequiredField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string ReturnMatchCode
        {
            get
            {
                return this.returnMatchCodeField;
            }
            set
            {
                this.returnMatchCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public bool GEO_LOCATION_TREE_REQUIRED
        {
            get
            {
                return this.gEO_LOCATION_TREE_REQUIREDField;
            }
            set
            {
                this.gEO_LOCATION_TREE_REQUIREDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public string GEO_LOCATION_NAME
        {
            get
            {
                return this.gEO_LOCATION_NAMEField;
            }
            set
            {
                this.gEO_LOCATION_NAMEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public string GEO_LOCATION_ID
        {
            get
            {
                return this.gEO_LOCATION_IDField;
            }
            set
            {
                this.gEO_LOCATION_IDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=17)]
        public bool NotesRequired
        {
            get
            {
                return this.notesRequiredField;
            }
            set
            {
                this.notesRequiredField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=18)]
        public enmBestSeller BestSeller
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BestSellerSpecified
        {
            get
            {
                return this.bestSellerFieldSpecified;
            }
            set
            {
                this.bestSellerFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=19)]
        public bool ReturnAppliedChargingPolicyDetails
        {
            get
            {
                return this.returnAppliedChargingPolicyDetailsField;
            }
            set
            {
                this.returnAppliedChargingPolicyDetailsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=20)]
        public int IMAGENOTREQUIRED
        {
            get
            {
                return this.iMAGENOTREQUIREDField;
            }
            set
            {
                this.iMAGENOTREQUIREDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=21)]
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=22)]
        public System.Nullable<int> NationalityID
        {
            get
            {
                return this.nationalityIDField;
            }
            set
            {
                this.nationalityIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=23)]
        public bool ReturnAttachedOptionExtra
        {
            get
            {
                return this.returnAttachedOptionExtraField;
            }
            set
            {
                this.returnAttachedOptionExtraField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=24)]
        public bool RETURNCHEAPESTPRICE
        {
            get
            {
                return this.rETURNCHEAPESTPRICEField;
            }
            set
            {
                this.rETURNCHEAPESTPRICEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=25)]
        public string UseMultiplePricesByBookingAndPriceType
        {
            get
            {
                return this.useMultiplePricesByBookingAndPriceTypeField;
            }
            set
            {
                this.useMultiplePricesByBookingAndPriceTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=26)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order=27)]
        public UIPricesAndAvailabilitiesRequestModifiers Modifiers
        {
            get
            {
                return this.modifiersField;
            }
            set
            {
                this.modifiersField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=28)]
        public ServiceSearchType ServiceSearchType
        {
            get
            {
                return this.serviceSearchTypeField;
            }
            set
            {
                this.serviceSearchTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=29)]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ReturnMandatoryExtraPrices
        {
            get
            {
                return this.returnMandatoryExtraPricesField;
            }
            set
            {
                this.returnMandatoryExtraPricesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=30)]
        [System.ComponentModel.DefaultValueAttribute(false)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order=31)]
        public int IsPackageService
        {
            get
            {
                return this.isPackageServiceField;
            }
            set
            {
                this.isPackageServiceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=32)]
        public bool ApplyBTPTFilter
        {
            get
            {
                return this.applyBTPTFilterField;
            }
            set
            {
                this.applyBTPTFilterField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=33)]
        public EstimatedServiceRatesInfo EstimatedServiceRatesInfo
        {
            get
            {
                return this.estimatedServiceRatesInfoField;
            }
            set
            {
                this.estimatedServiceRatesInfoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=34)]
        public RulesValidationModifier ValidateRulesModifier
        {
            get
            {
                return this.validateRulesModifierField;
            }
            set
            {
                this.validateRulesModifierField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=35)]
        public bool CalledFromFIT
        {
            get
            {
                return this.calledFromFITField;
            }
            set
            {
                this.calledFromFITField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=36)]
        public System.Nullable<int> ProposalEnquiryId
        {
            get
            {
                return this.proposalEnquiryIdField;
            }
            set
            {
                this.proposalEnquiryIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=37)]
        public bool IsLinkedService
        {
            get
            {
                return this.isLinkedServiceField;
            }
            set
            {
                this.isLinkedServiceField = value;
            }
        }

        public UIPaxOccupancy[] PaxOccupancies;
        public UIChildren CHILDREN;
    

    }
     public partial class UIPricesAndAvailabilitiesRequestModifiers
    {
        
        private enmReturnWarnings rETURNWARNINGSField;
        
        private bool dONOTFETCHNONMEALPLANSField;
        
        private bool returnServiceFacilitiesField;
        
        private bool returnServiceRatingsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public enmReturnWarnings RETURNWARNINGS
        {
            get
            {
                return this.rETURNWARNINGSField;
            }
            set
            {
                this.rETURNWARNINGSField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public bool DONOTFETCHNONMEALPLANS
        {
            get
            {
                return this.dONOTFETCHNONMEALPLANSField;
            }
            set
            {
                this.dONOTFETCHNONMEALPLANSField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public bool ReturnServiceFacilities
        {
            get
            {
                return this.returnServiceFacilitiesField;
            }
            set
            {
                this.returnServiceFacilitiesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool ReturnServiceRatings
        {
            get
            {
                return this.returnServiceRatingsField;
            }
            set
            {
                this.returnServiceRatingsField = value;
            }
        }
    }
    public enum enmReturnWarnings
    {
        
        /// <remarks/>
        NONE,
        
        /// <remarks/>
        ALL,
    }

 [Serializable]
    public class UIPaxOccupancy
    {
        public int OccupanyTypeID;
        public int NoofPax;
        public int NoofChildSharingPax;
        public int NoofChildNonSharingPax;
    }
    [Serializable]
    public class UIChildren
    {
        public UIAges[] AGES;
    }

    [Serializable]
    public class UIAges
    {
        public int AGE;
        public int COUNT;
        public int BookedChildRateID;
        public double SellPrice;
        public double ChildAgeBandSellingPercentage;
        public double PackageOptionChildCostAmount;
        public double PackageOptionChildCostTax;
        public double PackageOptionChildCostPercent;
        public double PPCostAmount;
        public bool IsPackageChildRatePresent;
        public bool IsPackageOptionChildCostExists;
        public bool IsChildAgeBandPresent;
        public string DateOfBirth;
        public int OccupanyTypeID;
        public int PaxID;
        public bool? IsSharing;
    }
    // public partial class UIPricesAndAvailabilityRequest
    // {
        
    //     private string serviceCodeField;
        
    //     private int serviceIDField;
        
    //     private int serviceTypeIDField;
        
    //     private bool isNonHotelTypeField;
        
    //     private bool availableOnlyField;
        
    //     private System.DateTime bookingStartDateField;
        
    //     private System.DateTime bookingEndDateField;
        
    //     private short noOfNightsField;
        
    //     private enmRoom_Reply room_ReplyField;
        
    //     private RoomDetails[] roomDetailsField;
        
    //     private InsuranceDetails insuranceDetailsField;
        
    //     private string currencyField;
        
    //     private bool requireEstimatedPricesField;
        
    //     private int bookingTypeField;
        
    //     private int priceTypeField;
        
    //     private string bookingRefNoField;
        
    //     private bool isFastBuildServiceField;
        
    //     private int leadPassengerCountryIDField;
        
    //     private bool isEstimatedServiceField;
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    //     public string ServiceCode
    //     {
    //         get
    //         {
    //             return this.serviceCodeField;
    //         }
    //         set
    //         {
    //             this.serviceCodeField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    //     public int ServiceID
    //     {
    //         get
    //         {
    //             return this.serviceIDField;
    //         }
    //         set
    //         {
    //             this.serviceIDField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=2)]
    //     public int ServiceTypeID
    //     {
    //         get
    //         {
    //             return this.serviceTypeIDField;
    //         }
    //         set
    //         {
    //             this.serviceTypeIDField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=3)]
    //     public bool IsNonHotelType
    //     {
    //         get
    //         {
    //             return this.isNonHotelTypeField;
    //         }
    //         set
    //         {
    //             this.isNonHotelTypeField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=4)]
    //     public bool AvailableOnly
    //     {
    //         get
    //         {
    //             return this.availableOnlyField;
    //         }
    //         set
    //         {
    //             this.availableOnlyField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=5)]
    //     public System.DateTime BookingStartDate
    //     {
    //         get
    //         {
    //             return this.bookingStartDateField;
    //         }
    //         set
    //         {
    //             this.bookingStartDateField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=6)]
    //     public System.DateTime BookingEndDate
    //     {
    //         get
    //         {
    //             return this.bookingEndDateField;
    //         }
    //         set
    //         {
    //             this.bookingEndDateField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=7)]
    //     public short NoOfNights
    //     {
    //         get
    //         {
    //             return this.noOfNightsField;
    //         }
    //         set
    //         {
    //             this.noOfNightsField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=8)]
    //     public enmRoom_Reply Room_Reply
    //     {
    //         get
    //         {
    //             return this.room_ReplyField;
    //         }
    //         set
    //         {
    //             this.room_ReplyField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlArrayAttribute(Order=9)]
    //     public RoomDetails[] RoomDetails
    //     {
    //         get
    //         {
    //             return this.roomDetailsField;
    //         }
    //         set
    //         {
    //             this.roomDetailsField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=10)]
    //     public InsuranceDetails InsuranceDetails
    //     {
    //         get
    //         {
    //             return this.insuranceDetailsField;
    //         }
    //         set
    //         {
    //             this.insuranceDetailsField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=11)]
    //     public string Currency
    //     {
    //         get
    //         {
    //             return this.currencyField;
    //         }
    //         set
    //         {
    //             this.currencyField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=12)]
    //     public bool RequireEstimatedPrices
    //     {
    //         get
    //         {
    //             return this.requireEstimatedPricesField;
    //         }
    //         set
    //         {
    //             this.requireEstimatedPricesField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=13)]
    //     public int BookingType
    //     {
    //         get
    //         {
    //             return this.bookingTypeField;
    //         }
    //         set
    //         {
    //             this.bookingTypeField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=14)]
    //     public int PriceType
    //     {
    //         get
    //         {
    //             return this.priceTypeField;
    //         }
    //         set
    //         {
    //             this.priceTypeField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=15)]
    //     public string BookingRefNo
    //     {
    //         get
    //         {
    //             return this.bookingRefNoField;
    //         }
    //         set
    //         {
    //             this.bookingRefNoField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=16)]
    //     public bool IsFastBuildService
    //     {
    //         get
    //         {
    //             return this.isFastBuildServiceField;
    //         }
    //         set
    //         {
    //             this.isFastBuildServiceField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=17)]
    //     public int LeadPassengerCountryID
    //     {
    //         get
    //         {
    //             return this.leadPassengerCountryIDField;
    //         }
    //         set
    //         {
    //             this.leadPassengerCountryIDField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=18)]
    //     public bool IsEstimatedService
    //     {
    //         get
    //         {
    //             return this.isEstimatedServiceField;
    //         }
    //         set
    //         {
    //             this.isEstimatedServiceField = value;
    //         }
    //     }
    // }
    // public partial class InsuranceDetails
    // {
        
    //     private OptionID[] serviceOptionsField;
        
    //     private PaxTypeID[] paxTypesField;
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlArrayAttribute(Order=0)]
    //     public OptionID[] ServiceOptions
    //     {
    //         get
    //         {
    //             return this.serviceOptionsField;
    //         }
    //         set
    //         {
    //             this.serviceOptionsField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
    //     public PaxTypeID[] PaxTypes
    //     {
    //         get
    //         {
    //             return this.paxTypesField;
    //         }
    //         set
    //         {
    //             this.paxTypesField = value;
    //         }
    //     }
    // }
      //public partial class PaxTypeID
    // {
        
    //     private int countField;
        
    //     private string valueField;
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlAttributeAttribute()]
    //     public int Count
    //     {
    //         get
    //         {
    //             return this.countField;
    //         }
    //         set
    //         {
    //             this.countField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlTextAttribute()]
    //     public string Value
    //     {
    //         get
    //         {
    //             return this.valueField;
    //         }
    //         set
    //         {
    //             this.valueField = value;
    //         }
    //     }
    // }

    #region  enums
    public enum ServiceSearchType
    {
        
        /// <remarks/>
        Enhanced,
        
        /// <remarks/>
        MultipleBTPT,
    }
     public enum enmBestSeller
    {
        
        /// <remarks/>
        IgnoreBestsellerTag,
        
        /// <remarks/>
        OnlyBestseller,
        
        /// <remarks/>
        NotBestseller,
    }
    public enum enmReturnPrices
    {
        
        /// <remarks/>
        OptionsPerDayPrice,
        
        /// <remarks/>
        OptionsHavingMinimumPrice,
    }

    //  public enum enmRoom_Reply
    // {
        
    //     /// <remarks/>
    //     ALL_ROOM,
        
    //     /// <remarks/>
    //     ANY_ROOM,
        
    //     /// <remarks/>
    //     ALL_OPTION,
    // }
    #endregion

  //   public partial class OptionID
    // {
        
    //     private int valueField;
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlTextAttribute()]
    //     public int Value
    //     {
    //         get
    //         {
    //             return this.valueField;
    //         }
    //         set
    //         {
    //             this.valueField = value;
    //         }
    //     }
    // }

    //  public partial class RoomDetails
    // {
        
    //     private int noOfAdultField;
        
    //     private int noOfChildrenField;
        
    //     private ExtraBed[] extraBedAgesField;
        
    //     private int noOfRoomsField;
        
    //     private string priceCodeField;
        
    //     private string roomTypeField;
        
    //     private int optionIdField;
        
    //     private ChildNotSharingAges[] childNotSharingAgesField;
        
       // private int occupancyField;
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        // public int NoOfAdult
        // {
        //     get
        //     {
        //         return this.noOfAdultField;
        //     }
        //     set
        //     {
        //         this.noOfAdultField = value;
        //     }
        // }
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        // public int NoOfChildren
        // {
        //     get
        //     {
        //         return this.noOfChildrenField;
        //     }
        //     set
        //     {
        //         this.noOfChildrenField = value;
        //     }
        // }
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlArrayAttribute(Order=2)]
        // public ExtraBed[] ExtraBedAges
        // {
        //     get
        //     {
        //         return this.extraBedAgesField;
        //     }
        //     set
        //     {
        //         this.extraBedAgesField = value;
        //     }
        // }
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        // public int NoOfRooms
        // {
        //     get
        //     {
        //         return this.noOfRoomsField;
        //     }
        //     set
        //     {
        //         this.noOfRoomsField = value;
        //     }
        // }
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        // public string PriceCode
        // {
        //     get
        //     {
        //         return this.priceCodeField;
        //     }
        //     set
        //     {
        //         this.priceCodeField = value;
        //     }
        // }
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        // public string RoomType
        // {
        //     get
        //     {
        //         return this.roomTypeField;
        //     }
        //     set
        //     {
        //         this.roomTypeField = value;
        //     }
        // }
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        // public int OptionId
        // {
        //     get
        //     {
        //         return this.optionIdField;
        //     }
        //     set
        //     {
        //         this.optionIdField = value;
        //     }
        // }
        
        /// <remarks/>
        // [System.Xml.Serialization.XmlArrayAttribute(Order=7)]
        // public ChildNotSharingAges[] ChildNotSharingAges
        // {
        //     get
        //     {
        //         return this.childNotSharingAgesField;
        //     }
        //     set
        //     {
        //         this.childNotSharingAgesField = value;
        //     }
        // }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=8)]
    //     public int Occupancy
    //     {
    //         get
    //         {
    //             return this.occupancyField;
    //         }
    //         set
    //         {
    //             this.occupancyField = value;
    //         }
    //     }
    // }
    //  public partial class ExtraBed
    // {
        
    //     private int roomField;
        
    //     private int ageField;
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    //     public int Room
    //     {
    //         get
    //         {
    //             return this.roomField;
    //         }
    //         set
    //         {
    //             this.roomField = value;
    //         }
    //     }
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=1)]
    //     public int Age
    //     {
    //         get
    //         {
    //             return this.ageField;
    //         }
    //         set
    //         {
    //             this.ageField = value;
    //         }
    //     }
    // }
    // public partial class ChildNotSharingAges
    // {
        
    //     private int ageField;
        
    //     /// <remarks/>
    //     [System.Xml.Serialization.XmlElementAttribute(Order=0)]
    //     public int Age
    //     {
    //         get
    //         {
    //             return this.ageField;
    //         }
    //         set
    //         {
    //             this.ageField = value;
    //         }
    //     }
    // }
}