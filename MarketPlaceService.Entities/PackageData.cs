using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class PackageData
    {
        public int PackageId { get; set; }
        public string LongName { get; set; }
        public string DisplayName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string CurrencyIsoCode { get; set; }
        public int RegionId { get; set; }
 		public string RegionName { get; set; }
        public int PackageStatusId { get; set; }
        public string PackageStatusName{get;set;}
        public string TermsInclusion { get; set; }
        public string TermsExclusion { get; set; }
        public string TimeOfService { get; set; }
        public string PackageDurationOfService { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Term> TermList { get; set; }
        public List<Note> Notes { get; set; }
        public List<ElementData> Elements { get; set; }
        public List<OptionalData> Optionals { get; set; }
        public List<FacilityData> Facilities { get; set; }
        public List<ChangeDetected> ChangesDetected { get; set; }
        public ApplicableRating ApplicableRatings { get; set; }
        public List<ItineraryText> ItineraryText { get; set; }
        public List<PackageItinerary> PackageItinerary {get;set;} 
        public int PackageTypeId { get; set; }
        public string PackageTypeName {get;set;}
        public int SearchPriority { get; set; }
        public string ServiceDuration { get; set; }
        //public int? PickupOptionId { get; set; }
        //public int? DropoffOptionId { get; set; }
        public List<Region> AdditionalRegions { get; set; }
    }

    public class ElementData : OptionExtra
    {

    }

    public class OptionalData : OptionExtra
    {
        public int OptionExtraId { get; set; }
        public bool IsOptionalOption { get; set; }
        public string ServiceName { get; set; }
        public bool Mandatory { get; set; }
    }

}
