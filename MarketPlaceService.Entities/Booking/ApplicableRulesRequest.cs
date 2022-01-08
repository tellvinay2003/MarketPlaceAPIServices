using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class ApplicableRulesRequest
    {
        public ApplicableRulesRequestData GetApplicableRulesRequestData { get; set; }
    }

    public class ApplicableRulesRequestData
    {
        public int ClientID { get; set; }
        public int BookingTypeID { get; set; }
        public int PriceTypeID { get; set; }
        public RuleTypeModifier ApplicableRuleType { get; set; }
        public List<ApplicableRuleService> ServiceInfo { get; set; }
        public string BookingRefNo { get; set; }
        public int? BookingID { get; set; }
        public bool? ReturnAllApplicableRules { get; set; }
        public bool? ReturnEnforcedRulesOnly { get; set; }
        public Guid SiteId { get; set; }
        public Guid SubscriberId { get; set; }
        public int OrganisationId { get; set; }
        public DateTime RuleFromDate { get; set; }
        public DateTime RuleToDate { get; set; }
    }

    public class RuleTypeModifier
    {
        public bool? Exclusion { get; set; }
        public bool? MinimumStay { get; set; }
        public bool? Restriction { get; set; }
        public bool? AllocationRestriction { get; set; }
        public bool? SpecialOffer { get; set; }
        public bool? BookingPeriod { get; set; }
        public bool? Occupancy { get; set; }
        public bool? MinimumAge { get; set; }
        public bool? ChildDiscounts { get; set; }
        public bool? CircuitOffer { get; set; }
        public bool? PassengerRestriction { get; set; }
    }

    public class ApplicableRuleService
    {
        public int ServiceID { get; set; }
        public int? BookedServiceID { get; set; }
        public List<ApplicableRuleOptionExtra> ServiceOptionExtraInfo { get; set; }
        public bool? IsEstimatedService { get; set; }
        public bool? IsRestrictOutput { get; set; }
        public bool? IsShowFilteredRules { get; set; }
        public Guid MarketplaceProductId { get; set; }
        public Guid SubscriberId { get; set; }
        public Guid SiteId { get; set; }
        public int AgentId { get; set; }
        public short Producttypeid { get; set; }
    }

    public class ApplicableRuleOptionExtra
    {
        public int OptionExtraID { get; set; }
        public bool IsOption { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int NoOfNights { get; set; }
        public int Quantity { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public string ChildAges { get; set; }
        public int BuyPriceID { get; set; }
        public int SellPriceID { get; set; }
        public bool? DayOverlap { get; set; }
        public string Guid { get; set; }
    }
}
