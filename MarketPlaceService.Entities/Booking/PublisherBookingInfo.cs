using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class PublisherBookingInfo
    {
        public int BookingId { get; set; }
        public string BookingReferenceNumber { get; set; }
        public string BookingName { get; set; }
        public List<PassengerData> BookingPassengers { get; set; }
        public List<PublisherBookedServiceData> BookedServices { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
    }

    public class PublisherBookedServiceData
    {
        public int ServiceId { get; set; }
        public int bookedServiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SupplierReference { get; set; }
        public List<PublisherBookedOptionData> BookedOptions { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
        public PickupDropOff PickupDropoffDetails { get; set; }
    }

    public class PublisherBookedOptionData : BookedOptionData
    {
        public PriceData Price { get; set; }
        public int BookedOptionStatusId { get; set; }
        
    }

    public class PriceData
    {
        public  decimal BookedOptionCostAmount { get; set; }
        public decimal BookedOptionSellAmount { get; set; }
        public decimal BookedOptionTotalCostAmount { get; set; }
        public decimal BookedOptionTotalSellAmount { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
    }

    public class PickupDropOff
    {
        public string PickUpTime { get; set; }
        public string PickUpDescription { get; set; }
        public string DropOffTime { get; set; }
        public string DropOffDescription { get; set; }
        public List<ChangeDetected> changesDetected { get; set; }
    }
}
