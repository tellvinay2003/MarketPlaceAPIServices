using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.TSv2ApiEntities
{
    public class GetServiceExtraPricesRequest : BaseRequest
    {
        private int pRICE_TYPE_IDField;

        private Vehicle vEHICLEField;

        private string priceCodeField;

        private bool isEstimatedServiceField;

        private int sERVICEIDField;

        private System.DateTime fROMDATEField;

        private System.DateTime tODATEField;

        private bool returnLinkedServiceOptionsField;

        private ExtraDetails[] extrasRequiredField;

        private bool iGNORECHILDAGEField;

        private bool rETURNONLYNONACCOMODATIONSERVICESField;

        private bool aPPLYEXCHANGERATESField;

        private string cURRENCYISOCODEField;

        private int clientIdField;

        private bool returnAppliedChargingPolicyDetailsField;

        private string countryCodeForPassengerTaxField;

        private System.Nullable<int> proposalEnquiryIdField;

        private EstimatedServiceRatesInfo estimatedServiceRatesInfoField;

        private RulesValidationModifier validateRulesModifierField;


        public GetServiceExtraPricesRequest()
        {
            this.isEstimatedServiceField = false;
        }
      

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int BOOKING_TYPE_ID { get; set; }
       

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public Vehicle VEHICLE
        {
            get
            {
                return this.vEHICLEField;
            }
            set
            {
                this.vEHICLEField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public int SERVICEID
        {
            get
            {
                return this.sERVICEIDField;
            }
            set
            {
                this.sERVICEIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public System.DateTime FROMDATE
        {
            get
            {
                return this.fROMDATEField;
            }
            set
            {
                this.fROMDATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public System.DateTime TODATE
        {
            get
            {
                return this.tODATEField;
            }
            set
            {
                this.tODATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public bool ReturnLinkedServiceOptions
        {
            get
            {
                return this.returnLinkedServiceOptionsField;
            }
            set
            {
                this.returnLinkedServiceOptionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ExtraDetail")]
        public ExtraDetails[] ExtrasRequired
        {
            get
            {
                return this.extrasRequiredField;
            }
            set
            {
                this.extrasRequiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool IGNORECHILDAGE
        {
            get
            {
                return this.iGNORECHILDAGEField;
            }
            set
            {
                this.iGNORECHILDAGEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool RETURNONLYNONACCOMODATIONSERVICES
        {
            get
            {
                return this.rETURNONLYNONACCOMODATIONSERVICESField;
            }
            set
            {
                this.rETURNONLYNONACCOMODATIONSERVICESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public bool APPLYEXCHANGERATES
        {
            get
            {
                return this.aPPLYEXCHANGERATESField;
            }
            set
            {
                this.aPPLYEXCHANGERATESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 14)]
        public string CURRENCYISOCODE
        {
            get
            {
                return this.cURRENCYISOCODEField;
            }
            set
            {
                this.cURRENCYISOCODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public int ClientId
        {
            get
            {
                return this.clientIdField;
            }
            set
            {
                this.clientIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 18)]
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 19)]
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 20)]
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

        
        public bool CalledFromFIT { get; set; }       
        public Guid MarketplaceProductId { get; set; }
        public Guid SubscriberId { get; set; }
        public Guid SiteId { get; set; }
        public int OrganisationId { get; set; }
        public short Producttypeid { get; set; }

    }

    public partial class ExtraDetails
    {

        private ExtraChildDetail[] cHILDRENField;

        private string remoteExtraCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int OccupancyID { get; set; }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int Quantity { get; set; }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int Adults { get; set; }


        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CHILD_RATE")]
        public ExtraChildDetail[] CHILDREN
        {
            get
            {
                return this.cHILDRENField;
            }
            set
            {
                this.cHILDRENField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string RemoteExtraCode { get; set; }

    }
    public partial class ExtraChildDetail
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ChildQuantity { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChildAge { get; set; }
    }
    public partial class Vehicle
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool AvailableOnly { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DriverDetails DriverDetails { get; set; }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public Location PickUpLocation { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public Location DropOffLocation { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string PriceCode { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string ServiceTypeID { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public bool IsRecommendedProduct { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string Currency { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public int LargeLuggage { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public int SmallLuggage { get; set; }
    }
    public partial class Location
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public enmTranferType Type { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string LocationCode { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Date { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Time { get; set; }
    }
    public enum enmTranferType
    {
        Airport,
        Station,
        Port,
        Hotel,
        Location,
        Any,
        Stations
    }
    public partial class DriverDetails
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Age { get; set; }
    }
    public partial class EstimatedServiceRatesInfo
    {

        private int priceIdField;

        private decimal costIncreaseField;

        private decimal sellMarginField;

        private string occupancyMatrixField;

        private bool validateRulesField;

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
        public decimal CostIncrease
        {
            get
            {
                return this.costIncreaseField;
            }
            set
            {
                this.costIncreaseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public decimal SellMargin
        {
            get
            {
                return this.sellMarginField;
            }
            set
            {
                this.sellMarginField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string OccupancyMatrix
        {
            get
            {
                return this.occupancyMatrixField;
            }
            set
            {
                this.occupancyMatrixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool ValidateRules
        {
            get
            {
                return this.validateRulesField;
            }
            set
            {
                this.validateRulesField = value;
            }
        }
    }
    public partial class RulesValidationModifier
    {

        private bool skipMinimumAgeRuleField;

        private bool skipOccupancyRuleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool SkipMinimumAgeRule
        {
            get
            {
                return this.skipMinimumAgeRuleField;
            }
            set
            {
                this.skipMinimumAgeRuleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool SkipOccupancyRule
        {
            get
            {
                return this.skipOccupancyRuleField;
            }
            set
            {
                this.skipOccupancyRuleField = value;
            }
        }
    }
    public partial class BaseRequest
    {

        private AUTHENTICATE authenticateField;

        private string xMLResponseField;

        private string xMLRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public AUTHENTICATE Authenticate
        {
            get
            {
                return this.authenticateField;
            }
            set
            {
                this.authenticateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string XMLResponse
        {
            get
            {
                return this.xMLResponseField;
            }
            set
            {
                this.xMLResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string XMLRequest
        {
            get
            {
                return this.xMLRequestField;
            }
            set
            {
                this.xMLRequestField = value;
            }
        }
    }
    public partial class AUTHENTICATE
    {

        private string lICENSEKEYField;

        private string lANGUAGEField;

        private int pASSENGERIDField;

        private int tS_USERIDField;

        private enmConnector connectorField;

        private string cultureField;

        public AUTHENTICATE()
        {
            this.pASSENGERIDField = 0;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string LICENSEKEY
        {
            get
            {
                return this.lICENSEKEYField;
            }
            set
            {
                this.lICENSEKEYField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string LANGUAGE
        {
            get
            {
                return this.lANGUAGEField;
            }
            set
            {
                this.lANGUAGEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int PASSENGERID
        {
            get
            {
                return this.pASSENGERIDField;
            }
            set
            {
                this.pASSENGERIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int TS_USERID
        {
            get
            {
                return this.tS_USERIDField;
            }
            set
            {
                this.tS_USERIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public enmConnector Connector
        {
            get
            {
                return this.connectorField;
            }
            set
            {
                this.connectorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Culture
        {
            get
            {
                return this.cultureField;
            }
            set
            {
                this.cultureField = value;
            }
        }
    }
    public enum enmConnector
    {

        /// <remarks/>
        enmTSHotelAPI,

        /// <remarks/>
        enmTS,

        /// <remarks/>
        enmAOT,

        /// <remarks/>
        enmGTA,
    }
}
