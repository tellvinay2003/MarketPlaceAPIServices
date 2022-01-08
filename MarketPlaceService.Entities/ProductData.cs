using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class ProductData
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int MaximumOccupancy { get; set; }
        public int SearchPriority { get; set; }
        public int? PickupOptionId { get; set; }
        public int? DropoffOptionId { get; set; }
        public string TermsInclusion { get; set; }
        public string TermsExclusion { get; set; }
        public string TimeOfService { get; set; }
        public string ServiceDuration { get; set; }
        public bool IsRecommendedProduct { get; set; }
        public bool BestSeller { get; set; }
        public int CommunicationTypeId { get; set; }
        public int Status { get; set; }
		public string StatusName { get; set; }
        public string CurrencyISO { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public DateTime? Allocationreleasetime { get; set; }
        public List<Region> AdditionalRegions  {get; set; }
        public Supplier Supplier { get; set; }
        public List<Address> Addresses{get;set;}
        public List<OptionData> Options { get; set; }
        public List<ExtraData> Extras { get; set; }
        public List<FacilityData> Facilities {get;set;}
        public ProductCode ProductCode { get; set; }

        public ApplicableRating ApplicableRatings { get; set; }
        public List<Term> TermList { get; set; }
        public List<Note> Notes { get; set; }
        public List<ItineraryText> ItineraryText { get; set; }
       // public List<PackageItinerary> PackageItinerary {get;set;}
        public ServiceAdditionalInfo AdditonalInfo { get; set; }
        public bool? ServicePickupDropoffReadOnly{get;set;}

        public bool ImportSupplierAddress{get;set;}
      
    }

    public class ServiceClosureData
    {
        public bool? Never { get; set; }
        public Date When { get; set; }
    }   

    public class ServiceAdditionalInfo
    {
        public string PrimaryAirport { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public int? YearOfOpening { get; set; }
        public int? CompassDirection { get; set; }
        public string NearestAirport { get; set; }
        public decimal? ServiceDistance { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public ServiceClosureData ServiceClosure { get; set; }
        public DateTime? CheckinToTime{get;set;}
    }

    public class Term
    {
        public int TermId {get;set;}
        public PickUpDropOff PickUp { get; set; }
        public PickUpDropOff DropOff { get; set; }
    }

    public class Note
    {
        public int Id { get; set; }
        public int NoteTypeId { get; set; }
        public int NoteStatusId { get; set; }
        public string NoteStatusName {get;set;}
        public DateTime NoteDate { get; set; }
        public DateTime NoteEventDate { get; set; }
        public string NoteOrigin { get; set; }
        public string NoteText { get; set; }
        public string NoteSubject { get; set; }
        public DateTime NoteEndDate {get; set;}
    }

    public class ItineraryText
    {
        public int Id { get; set; }
        public int ItineraryTypeId { get; set; }
        public string ItineraryTypeName { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        
        public Date Dates { get; set; }
        public string Text { get; set; }
        public int? ShortCode { get; set; }
        public string Name { get; set; }
        public bool? MultiDay { get; set; }
        public int? AppliedType { get; set; }
    }

    public class PackageItinerary
    {
        public int ItineraryTypeId { get; set; }
        public string ItineraryTypeName { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }        
        public Date Dates { get; set; }
        public string Name { get; set; }
        public int? AppliedType { get; set; }

        public List<PackageDayItinerary> PackageDayItinerary {get;set;}
    }

    public class PackageDayItinerary
    {
        public int SequentialId {get;set;}
        public int Id {get;set;} 
        public string text{get;set;}
        public string Description {get;set;}        
    }

    public class Date
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class PickUpDropOff
    {
        public string Time { get; set; }
        public string Description { get; set; }
    }

    public class ApplicableRating
    {
        public List<RatingType> Types { get; set; }
    }

    public class Rating
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    public class RatingType
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public List<Rating> Ratings { get; set; }
    }

    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Address
    {
        public int AddresstypeId { get; set; }
        public string AddressTypeName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostCode { get; set; }
        public string Tel { get; set; }
        public Country Country { get; set; }

        public string EmergencyTel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string SecondEmail { get; set; }
        public string URL { get; set; }
        public string DescriptionUrl { get; set; }
        public string UrlAlternateText { get; set; }
    }

    public class State
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class Country
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class OptionExtra
    {
        public int Id { get; set; }
        public bool? Active { get; set; }
        public string Name { get; set; }
        public bool IsOption { get; set; }
        public OptionExtraType Type{get;set;}

        public Status Status{get;set;}
        public List<DependentOption> DependentOptions { get; set; }
        public ProductCode ProductCode { get; set; }
        public List<Description> Descriptions { get; set; }
        public int BookedOptionQty { get; set; }
        public BookedOptionStatus BookedOptionStatus{get;set;}
    }

    public class OptionExtraType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int TypeId { get; set; }
        public ChargingPolicy ChargingPolicy { get; set; }
        public List<Occupancy> Occupancies{get;set;}
        public Class Class{get;set;}
    }

    public class ChargingPolicy
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Occupancy
    {
        public int TypeID { get; set; }
    }

    public class Class 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Status 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DependentOption
    {
        public int Id { get; set; }
    }

    public class ProductCode 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Description{
        
    }

    public class BookedOptionStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OptionData : OptionExtra{}
    public class ExtraData : OptionExtra
    {
        public bool? Mandatory { get; set; }
    }

    public class FacilityData
    {
        public int ServiceTypeFacilityId { get; set; }
        public int? FacilityTypeId {get; set;}
        public string FacilityTypeName {get;set;}
        public int FacilityId { get; set; }
        public string Name{ get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool InternetAvailable { get; set; }
    }    
}