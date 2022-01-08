using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.TSv2ApiEntities
{
    public class CalculateBookingPriceResponse
    {        
        public CalculateBookingPriceResp CalculateBookingPriceResponseData { get; set; }
    }

    public class CalculateBookingPriceResp
    {
        public List<CalcBookingPriceServicePrice> ServicePriceInfo { get; set; }
        //public List<PackagePriceInfo> PackageList { get; set; }
        public BookingPricesSummary BookingPricesSummary { get; set; }
        public List<PackagePriceInfo> PackageList { get; set; }
    }


    [Serializable]
    public class PackagePriceInfo
    {
        public int PackageID { get; set; }
        public string Available { get; set; }
        public int PackageDepartureId { get; set; }
        public DateTime? PackageDepartureDate { get; set; }
        public string PackageShortName { get; set; }
        public string PackageLongName { get; set; }
        public string PackageTypeName { get; set; }
        public List<PackageElementInfo> PackageElementsInfoList { get; set; }
        public List<PackageServiceInfo> PackageServicesInfoList { get; set; }
    }

    public class PackageElementInfo
    {
        public int PackageElementId { get; set; }
        public string PackageElementName { get; set; }
        public int PackageElementTypeId { get; set; }
        public int Quantity { get; set; }
        public int NoofAdults { get; set; }
        public int NoOfChildren { get; set; }
        public List<ChildInfo> ChildArray { get; set; }
        public decimal? AdultPriceAmount { get; set; }
        public decimal? TotalSellAmount { get; set; }
        public decimal? AdultCommissionValue { get; set; }
        public decimal? ChildCommissionValue { get; set; }
        public decimal? CommissionValue { get; set; }
        public string Availability_Status { get; set; }
    }

    public class ChildInfo
    {
        public int ChildAge { get; set; }
        public int ChildCount { get; set; }
        public decimal? ChildAmount { get; set; }
    }
    public class PackageServiceInfo
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTypeName { get; set; }
        public int RegionID { get; set; }
        public List<PackageServiceOption> PackageServiceOptions { get; set; }
    }

    public class PackageServiceOption
    {
        public int PackageElementId { get; set; }
        public int OptionID { get; set; }
        public string OptionType { get; set; }
        public int Quantity { get; set; }
        public int NoOfAdults { get; set; }
        public int NoOfChildren { get; set; }
        public List<ChildInfo> ChildArray { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Availability_Status { get; set; }
        public decimal? TotalSellAmount { get; set; }
        public decimal? AdultSellAmount { get; set; }
        public decimal? AdultCommissionValue { get; set; }
        public decimal? ChildCommissionValue { get; set; }
        public decimal? Commission { get; set; }
        public bool IsOptional { get; set; }
        public bool IsMandatoryOptional { get; set; }
        public decimal? SellAmount { get; set; }
        public bool IsExtra { get; set; }
        public bool DayOverlap { get; set; }
        public string MealPlanName { get; set; }
        public decimal? CommissionPercentage { get; set; }
    }
    public class CalcBookingPriceServicePrice
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTypeName { get; set; }
        public OptionInfo OptionInfo { get; set; }
        public ExtraInfo ExtraInfo { get; set; }
        public TotalIndividualServicePrices TotalIndividualServicePrices { get; set; }
        public bool IsEndpointService { get; set; } 

        [System.Xml.Serialization.XmlIgnore]
        public string StartDate { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public string EndDate { get; set; }
        public int? ServiceTypeID { get; set; }

        public bool isAccessNotAllowed {get; set;}
    }

    public class OptionInfo
    {
        public List<OptionResponse> Options { get; set; }
        public decimal TotalOptionSellPrice { get; set; }
        public decimal TotalOptionSellPriceAfterOffer { get; set; }
        public decimal TotalOptionSellPriceAfterDiscount { get; set; }
        public decimal TotalOptionCostPriceAmount { get; set; }
        public decimal TotalOptionOriginalSell { get; set; }
    }

    public class OptionResponse
    {
        public int UniqueIdentifier { get; set; }
        public int OptionID { get; set; }
        public string OptionName { get; set; }
        public decimal OptionSellPrice { get; set; }
        public decimal OptionSellPriceAfterOffer { get; set; }
        public decimal OptionSellPriceAfterDiscount { get; set; }
        public string ErrorMessage { get; set; }
        public decimal OptionCostPriceAmount { get; set; }
        public decimal OptionOriginalSell { get; set; }
        public string PriceCode { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public int PriceID { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public int AdultCount { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public int ChildCount { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public decimal OptionChildSellPrice { get; set; }

        public bool IsPricesNotFound { get; set; }
        public bool OptionIsAvailable { get; set; }
        public string MealPlanCode { get; set; }
    }

    public class ExtraInfo
    {
        public List<ExtraResponse> Extras { get; set; }
        public decimal TotalExtraSellPrice { get; set; }
        public decimal TotalExtraSellPriceAfterOffer { get; set; }
        public decimal TotalExtraSellPriceAfterDiscount { get; set; }
        public decimal TotalExtraCostPriceAmount { get; set; }
        public decimal TotalExtraOriginalSell { get; set; }
    }

    public class ExtraResponse
    {
        public int UniqueIdentifier { get; set; }
        public int ExtraID { get; set; }
        public string ExtraName { get; set; }
        public decimal ExtraSellPrice { get; set; }
        public decimal ExtraSellPriceAfterOffer { get; set; }
        public decimal ExtraSellPriceAfterDiscount { get; set; }
        public string ErrorMessage { get; set; }
        public decimal ExtraCostPriceAmount { get; set; }
        public decimal ExtraOriginalSell { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public int PriceID { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public int AdultCount { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public int ChildCount { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public decimal ExtraChildSellPrice { get; set; }

        public bool IsPricesNotFound { get; set; }
        public string MealPlanCode { get; set; }
    }

    public class TotalIndividualServicePrices
    {
        public decimal TotalPriceWithoutExtras { get; set; }
        public decimal TotalPriceWithoutExtrasAfterOffer { get; set; }
        public decimal TotalPriceWithoutExtrasAfterDiscount { get; set; }
        public decimal TotalPriceIncludingExtras { get; set; }
        public decimal TotalPriceIncludingExtrasAfterOffer { get; set; }
        public decimal TotalPriceIncludingExtrasAfterDiscount { get; set; }
    }

    public class BookingPricesSummary
    {
        public string CurrencyCode { get; set; }
        public decimal FinalBookingPrice { get; set; }
        public decimal FinalBookingPriceAfterOffer { get; set; }
        public decimal FinalBookingPriceAfterDiscount { get; set; }
        public decimal? InstalmentDiscountAppliedOn { get; set; }
        public BookingFee BookingFee { get; set; }
        public Discount Discount { get; set; }
        //EOC PoonamK(153)
    }

    public class BookingFee
    {
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

    public class Discount
    {
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }

}
