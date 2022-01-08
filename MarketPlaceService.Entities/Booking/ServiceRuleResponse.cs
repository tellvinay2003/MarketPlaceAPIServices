using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class ServiceRuleResponse
    {
        public GetApplicableRulesResponseData ApplicableRulesResponse { get; set; }
    }

    public class GetApplicableRulesResponseData
    {
        public List<ApplicableRuleServiceResponse> Services { get; set; }
    }

    public class ApplicableRuleServiceResponse
    {

        public int ServiceID { get; set; }
        public ApplicableRules ApplicableRules { get; set; }
    }

    public partial class ApplicableRules
    {
        public List<Exclusions> ExclusionRules { get; set; }
        public List<MinimumStays> MinimumStayRules { get; set; }
        public List<Restrictions> RestrictionRules { get; set; }
        public List<AllocationRestrictions> AllocationRestrictionRules { get; set; }
        public List<BookingPeriod> BookingPeriodRules { get; set; }
        public List<OccupancyRules> OccupancyRules { get; set; }
        public List<SpecialOffers> SpecialOffers { get; set; }
        public List<MinimumAge> MinimumAgeRules { get; set; }
        public List<ChildDiscounts> ChildDiscounts { get; set; }
        public List<CircuitOffer> CircuitOffers { get; set; }
        public List<PassengerRestrictions> PassengerRestrictionRules { get; set; }
    }

    public class Exclusions : ApplicableServiceRuleResponse
    {

    }
    public class MinimumStays : ApplicableServiceRuleResponse { }
    public class Restrictions : ApplicableServiceRuleResponse { }
    public class AllocationRestrictions : ApplicableServiceRuleResponse { }
    public class BookingPeriod : ApplicableServiceRuleResponse { }
    public class OccupancyRules : ApplicableServiceRuleResponse { }
    public class SpecialOffers : ApplicableServiceRuleResponse { }
    public class MinimumAge : ApplicableServiceRuleResponse { }
    public class ChildDiscounts : ApplicableServiceRuleResponse { }
    public class CircuitOffer : ApplicableServiceRuleResponse { }
    public class PassengerRestrictions : ApplicableServiceRuleResponse { }
}
