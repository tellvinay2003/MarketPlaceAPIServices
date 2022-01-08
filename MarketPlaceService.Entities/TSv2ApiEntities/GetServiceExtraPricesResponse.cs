using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.TSv2ApiEntities
{
    public class GetServiceExtraPricesResponse
    {       

            private UIServiceExtra[] responseListField;

            private RequestResponseError[] errorsField;

            private int serviceIdField;

            private string serviceNameField;

            private int serviceTypeIdField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
            [System.Xml.Serialization.XmlArrayItemAttribute("ServiceExtras")]
            public UIServiceExtra[] ResponseList
            {
                get
                {
                    return this.responseListField;
                }
                set
                {
                    this.responseListField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
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
            [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
            public int ServiceId
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
            [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
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
            [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
            public int ServiceTypeId
            {
                get
                {
                    return this.serviceTypeIdField;
                }
                set
                {
                    this.serviceTypeIdField = value;
                }
            }
        
    }
    public partial class UIServiceExtra
    {

        private int serviceExtraIdField;

        private string serviceTypeExtraNameField;

        private int occupancyTypeIDField;

        private int minAdultsField;

        private int maxAdultsField;

        private int minChildField;

        private int maxChildField;

        private double tOTALPRICEField;

        private int childMaxAgeField;

        private int serviceTypeTypeIDField;

        private string serviceTypeTypeNameField;

        private bool extraMandatoryField;

        private UIExtraServiceOption[] serviceOptionsField;

        private ServiceExtrasPrice[] extraPricesField;

        private string optionIDField;

        private string optionNameField;

        private string priceCodeField;

        private double commissionAmountField;

        private int specialOffersCountField;

        private int bookingTypeIDField;

        private int priceTypeIDField;

        private bool chargingPolicyMileageBasedField;

        private int statusIDField;

        private bool hasCircuitOfferField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int ServiceExtraId
        {
            get
            {
                return this.serviceExtraIdField;
            }
            set
            {
                this.serviceExtraIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ServiceTypeExtraName
        {
            get
            {
                return this.serviceTypeExtraNameField;
            }
            set
            {
                this.serviceTypeExtraNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int OccupancyTypeID
        {
            get
            {
                return this.occupancyTypeIDField;
            }
            set
            {
                this.occupancyTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int MinAdults
        {
            get
            {
                return this.minAdultsField;
            }
            set
            {
                this.minAdultsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public int MaxAdults
        {
            get
            {
                return this.maxAdultsField;
            }
            set
            {
                this.maxAdultsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public double TOTALPRICE
        {
            get
            {
                return this.tOTALPRICEField;
            }
            set
            {
                this.tOTALPRICEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public int ChildMaxAge
        {
            get
            {
                return this.childMaxAgeField;
            }
            set
            {
                this.childMaxAgeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public int ServiceTypeTypeID
        {
            get
            {
                return this.serviceTypeTypeIDField;
            }
            set
            {
                this.serviceTypeTypeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string ServiceTypeTypeName
        {
            get
            {
                return this.serviceTypeTypeNameField;
            }
            set
            {
                this.serviceTypeTypeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool ExtraMandatory
        {
            get
            {
                return this.extraMandatoryField;
            }
            set
            {
                this.extraMandatoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 12)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ServiceOption", IsNullable = false)]
        public UIExtraServiceOption[] ServiceOptions
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
        [System.Xml.Serialization.XmlArrayAttribute(Order = 13)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ServiceExtraPrice")]
        public ServiceExtrasPrice[] ExtraPrices
        {
            get
            {
                return this.extraPricesField;
            }
            set
            {
                this.extraPricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string OptionID
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public int StatusID
        {
            get
            {
                return this.statusIDField;
            }
            set
            {
                this.statusIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
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
    public partial class UIExtraServiceOption
    {

        private int idField;

        private string valueField;

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
    public partial class ServiceExtrasPrice
    {

        private int priceIdField;

        private System.DateTime priceDateField;

        private string currencyIsoCodeField;

        private string chargingPolicyNameField;

        private string childPolicyNameField;

        private double priceAmountField;

        private Tax_Code_[] tAX_CODESField;

        private Applied_Taxes_ aPPLIED_TAXESField;

        private double buyPriceField;

        private string mealPlanNameField;

        private string mealPlanCodeField;

        private ServiceExtrasChildPrice[] extrasChildPricesField;

        private double originalSellPriceField;

        private AppliedChargingPolicyDetails appliedChargingPolicyDetailsField;

        private double cOMMISSIONVALUEField;

        private bool fNOField;

        private string tAXField;

        private string gROSSSELLField;

        private string endPointCurrencyField;

        private string endPointCostField;

        private double originalCostPriceField;

        private int mealPlanIDField;

        private string costCurrencyIsoCodeField;

        private double totalInclusiveTaxAppliedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int PriceId
        {
            get
            {
                return this.priceIdField;
            }
            set
            {
                this.priceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime PriceDate
        {
            get
            {
                return this.priceDateField;
            }
            set
            {
                this.priceDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string CurrencyIsoCode
        {
            get
            {
                return this.currencyIsoCodeField;
            }
            set
            {
                this.currencyIsoCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ChildPolicyName
        {
            get
            {
                return this.childPolicyNameField;
            }
            set
            {
                this.childPolicyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
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
        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        [System.Xml.Serialization.XmlArrayItemAttribute("TAX_CODE")]
        public Tax_Code_[] TAX_CODES
        {
            get
            {
                return this.tAX_CODESField;
            }
            set
            {
                this.tAX_CODESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public Applied_Taxes_ APPLIED_TAXES
        {
            get
            {
                return this.aPPLIED_TAXESField;
            }
            set
            {
                this.aPPLIED_TAXESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public double BuyPrice
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
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

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        public ServiceExtrasChildPrice[] ExtrasChildPrices
        {
            get
            {
                return this.extrasChildPricesField;
            }
            set
            {
                this.extrasChildPricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public double OriginalSellPrice
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
        public double COMMISSIONVALUE
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public bool FNO
        {
            get
            {
                return this.fNOField;
            }
            set
            {
                this.fNOField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public double OriginalCostPrice
        {
            get
            {
                return this.originalCostPriceField;
            }
            set
            {
                this.originalCostPriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public string CostCurrencyIsoCode
        {
            get
            {
                return this.costCurrencyIsoCodeField;
            }
            set
            {
                this.costCurrencyIsoCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
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
    public partial class Tax_Code_
    {

        private string tAX_CODEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string TAX_CODE
        {
            get
            {
                return this.tAX_CODEField;
            }
            set
            {
                this.tAX_CODEField = value;
            }
        }
    }
    public partial class Applied_Taxes_
    {

        private double tAXField;

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
    }
    public partial class ServiceExtrasChildPrice
    {

        private int priceIdField;

        private int childRateAgeField;

        private System.DateTime childRateDateField;

        private double childRatePriceAmountField;

        private double cHILDCOMMISSIONVALUEField;

        private Tax_Code_[] tAX_CODESField;

        private Applied_Taxes_ aPPLIED_TAXESField;

        private double childRateCostPriceAmountField;

        private double childRateOriginalCostPriceAmountField;

        private double childRateOriginalPriceAmountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int PriceId
        {
            get
            {
                return this.priceIdField;
            }
            set
            {
                this.priceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int ChildRateAge
        {
            get
            {
                return this.childRateAgeField;
            }
            set
            {
                this.childRateAgeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.DateTime ChildRateDate
        {
            get
            {
                return this.childRateDateField;
            }
            set
            {
                this.childRateDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public double ChildRatePriceAmount
        {
            get
            {
                return this.childRatePriceAmountField;
            }
            set
            {
                this.childRatePriceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public double CHILDCOMMISSIONVALUE
        {
            get
            {
                return this.cHILDCOMMISSIONVALUEField;
            }
            set
            {
                this.cHILDCOMMISSIONVALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("TAX_CODE")]
        public Tax_Code_[] TAX_CODES
        {
            get
            {
                return this.tAX_CODESField;
            }
            set
            {
                this.tAX_CODESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public Applied_Taxes_ APPLIED_TAXES
        {
            get
            {
                return this.aPPLIED_TAXESField;
            }
            set
            {
                this.aPPLIED_TAXESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public double ChildRateCostPriceAmount
        {
            get
            {
                return this.childRateCostPriceAmountField;
            }
            set
            {
                this.childRateCostPriceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public double ChildRateOriginalCostPriceAmount
        {
            get
            {
                return this.childRateOriginalCostPriceAmountField;
            }
            set
            {
                this.childRateOriginalCostPriceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public double ChildRateOriginalPriceAmount
        {
            get
            {
                return this.childRateOriginalPriceAmountField;
            }
            set
            {
                this.childRateOriginalPriceAmountField = value;
            }
        }
    }
    public partial class AppliedChargingPolicyDetails
    {

        private AppliedChargingPolicy appliedChargingPolicyField;

        private AppliedChargingDuration appliedChargingDurationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AppliedChargingPolicy AppliedChargingPolicy
        {
            get
            {
                return this.appliedChargingPolicyField;
            }
            set
            {
                this.appliedChargingPolicyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AppliedChargingDuration AppliedChargingDuration
        {
            get
            {
                return this.appliedChargingDurationField;
            }
            set
            {
                this.appliedChargingDurationField = value;
            }
        }
    }
    public partial class AppliedChargingPolicy
    {

        private int chargingPolicyIDField;

        private string chargingPolicyNameField;

        private int chargingPolicyCapacityField;

        private int chargingPolicyMinimumBookingAmountField;

        private bool chargingPolicyDayOverlapField;

        private bool chargingPolicyRoomBasedField;

        private bool chargingPolicyMileageBasedField;

        private bool chargingPolicyInsuranceBasedField;

        private int chargingDurationIDField;

        private int remoteKeyField;

        private decimal chargingPolicyMinimumInsuranceAmountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int ChargingPolicyID
        {
            get
            {
                return this.chargingPolicyIDField;
            }
            set
            {
                this.chargingPolicyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int ChargingPolicyCapacity
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int ChargingPolicyMinimumBookingAmount
        {
            get
            {
                return this.chargingPolicyMinimumBookingAmountField;
            }
            set
            {
                this.chargingPolicyMinimumBookingAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool ChargingPolicyDayOverlap
        {
            get
            {
                return this.chargingPolicyDayOverlapField;
            }
            set
            {
                this.chargingPolicyDayOverlapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public bool ChargingPolicyRoomBased
        {
            get
            {
                return this.chargingPolicyRoomBasedField;
            }
            set
            {
                this.chargingPolicyRoomBasedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public int ChargingDurationID
        {
            get
            {
                return this.chargingDurationIDField;
            }
            set
            {
                this.chargingDurationIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public int RemoteKey
        {
            get
            {
                return this.remoteKeyField;
            }
            set
            {
                this.remoteKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public decimal ChargingPolicyMinimumInsuranceAmount
        {
            get
            {
                return this.chargingPolicyMinimumInsuranceAmountField;
            }
            set
            {
                this.chargingPolicyMinimumInsuranceAmountField = value;
            }
        }
    }
    public partial class AppliedChargingDuration
    {

        private int chargingDurationIDField;

        private string chargingDurationNameField;

        private double chargingDurationMinuteField;

        private double chargingDurationHourField;

        private double chargingDurationDayField;

        private double chargingDurationWeekField;

        private double chargingDurationMonthField;

        private int remoteKeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int ChargingDurationID
        {
            get
            {
                return this.chargingDurationIDField;
            }
            set
            {
                this.chargingDurationIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ChargingDurationName
        {
            get
            {
                return this.chargingDurationNameField;
            }
            set
            {
                this.chargingDurationNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public double ChargingDurationMinute
        {
            get
            {
                return this.chargingDurationMinuteField;
            }
            set
            {
                this.chargingDurationMinuteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public double ChargingDurationHour
        {
            get
            {
                return this.chargingDurationHourField;
            }
            set
            {
                this.chargingDurationHourField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public double ChargingDurationDay
        {
            get
            {
                return this.chargingDurationDayField;
            }
            set
            {
                this.chargingDurationDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public double ChargingDurationWeek
        {
            get
            {
                return this.chargingDurationWeekField;
            }
            set
            {
                this.chargingDurationWeekField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public double ChargingDurationMonth
        {
            get
            {
                return this.chargingDurationMonthField;
            }
            set
            {
                this.chargingDurationMonthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public int RemoteKey
        {
            get
            {
                return this.remoteKeyField;
            }
            set
            {
                this.remoteKeyField = value;
            }
        }
    }
    public partial class RequestResponseError
    {

        private int errorNoField;

        private int errorTypeField;

        private string errorMessageField;

        private string descriptionField;

        private System.DateTime errDateField;

        private string sourceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int ErrorNo
        {
            get
            {
                return this.errorNoField;
            }
            set
            {
                this.errorNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int ErrorType
        {
            get
            {
                return this.errorTypeField;
            }
            set
            {
                this.errorTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public System.DateTime ErrDate
        {
            get
            {
                return this.errDateField;
            }
            set
            {
                this.errDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }
}
