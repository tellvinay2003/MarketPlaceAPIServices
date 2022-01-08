using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class ServiceRuleRequest
    {
        public int ServiceID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //BOC Abhijit L.(1051)
        public string BookingRef { get; set; }
        public int BookingTypeID { get; set; }
        public int PriceTypeID { get; set; }
        public int? NoOfPax { get; set; }
        public int? OptionExtraID { get; set; }
        public bool? IsOption { get; set; }
        public ServiceRuleModifier ServiceRuleModifier { get; set; }
        public ApplicableRulesRequestData GetBookingApplicableRules { get; set; }
        //BOC Ritesh
        public int? BookingID { get; set; }
        //EOC Ritesh
        //BOC Naresh(2020)
        public int? ClientId { get; set; }
        public bool? IsEstimatedService { get; set; }
        public Guid SiteId { get; set; }
        public Guid SubscriberId { get; set; }
        public int OrganisationId { get; set; }
    }

    public class ServiceRuleModifier
    {
        public bool ReturnAllServiceRules { get; set; }
        public bool ReturnApplicableRules { get; set; }
        public bool ReturnApplicableSpecialOffers { get; set; } //Abhijit L.(1051)
        public bool? ReturnEnforcedRulesOnly { get; set; } //Ritesh
        public bool? ReturnAllApplicableRules { get; set; } //Ritesh
    }
}
