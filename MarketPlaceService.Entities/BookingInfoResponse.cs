using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class BookingInfoResponse
    {
        public int BookingId { get; set; }
        public string BookingReferenceNumber { get; set; }
        public string BookingName { get; set; }
        public int OrganisationId { get; set; }
        public int ClientId { get; set; }
        public string CurrencyISOCode { get; set; }
        public int BookingPrefixId { get; set; }
        public int BookingOwnerId { get; set; }
        public List<PassengerData> BookingPassengers { get; set; }
        public List<BookedServiceData> BookedServices { get; set; }
        public int MarketPlaceSiteBookingId {get;set;}
        public List<ChangeDetected> changesDetected { get; set; }
        public Guid SiteBookingId { get; set; }
        public List<BookingNote> Notes { get; set; }
    }

    public class BookingNote
    {
        public int NoteId { get; set; }
        public int NoteTypeId { get; set; }
        public int NoteStatusId { get; set; }
        public byte[] Attachment { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime NoteEventDate { get; set; }
        public DateTime? NoteEndDate { get; set; }
        public string AttachmentName { get; set; }
    }

    public class BookedServiceData
    {
        public int ServiceId { get; set; }
        public Guid MarketplaceProductId { get; set; }
        public Guid MarketplaceTsProductId { get; set; }
        public Guid PublisherSiteId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<BookedOptionData> BookedOptions { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
        public int? Mpbookedserviceid { get; set; }
        public short ProductTypeId{get;set;}
    }

    public class BookedOptionData
    {
        public int BookedOptionId { get; set; }
        public int? ServiceOptionId { get; set; }
        public int? ServiceExtraId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PriceCode { get; set; }
        public List<int> AssignedPassengers { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public List<BookedChildRateData> BookedChildRates { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
    }

    public class PassengerData
    {
        public int PassengerId { get; set; }
        public int? PassengerTypeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Title { get; set; }
        public int OccupancyTypeId { get; set; }
        public int GenderId { get; set; }
        public string Nationality { get; set; }
        public string PassengerNotes { get; set; }
        public bool IsMainPassenger { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
        public int PassengerLogicalRoomId { get; set; } 
    }
    public class BookedChildRateData
    {
        public short? Age { get; set; }
        public int Quantity { get; set; }
        public int MpBookedChildRateId { get; set; }
        public int? PassengerId { get; set; }
        public ChildPriceData ChildPrice { get; set; }
        public List<ChangeDetected> changesDetected { get; set; } 
    }
    public class ChildPriceData
    {
        public decimal BookedChildRateCostAmount { get; set; }
        public decimal BookedChildRateSellAmount { get; set; }
        public decimal BookedChildRateTotalCostAmount { get; set; }
        public decimal BookedChildRateTotalSellAmount { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
    }
}
