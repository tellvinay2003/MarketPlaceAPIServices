using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class ApplicableRulesResponse
    {
        public List<ApplicableServiceRuleResponse> Rules { get; set; }
    }

    public class ApplicableServiceRuleResponse
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string RuleMessage { get; set; }
        public DateTime RuleFromDate { get; set; }
        public DateTime RuleToDate { get; set; }
        public string RuleText { get; set; }
        public bool RuleEnforced { get; set; }
        public string RuleTypeName { get; set; }
        public string RuleType { get; set; }
        public string OptionExtraName { get; set; }
        public bool? IsOption { get; set; }
        public int? ServiceOptionExtraId { get; set; }
        public bool? RuleSuppressed { get; set; }
        public byte RuleBasedOn { get; set; }
        public int? AppliedRuleId { get; set; }
        public int? MaxChildAge { get; set; }
        public int? MaxPax { get; set; }
        public string LinkedOfferRuleIds { get; set; }
        public string Guid { get; set; }
        public bool? AppliedToBuyRates { get; set; }
        public bool? AppliedToSellRates { get; set; }
        public int ServiceId { get; set; }
        public string ServiceLongName { get; set; }
        public int BookedServiceId { get; set; }
        public string RegionName { get; set; }
        public int PackageServiceId { get; set; }
        public int  PackageOptionId  { get; set; }
        public int? PackageElementId  { get; set; }
        public bool IsPackageOptionOptional { get; set; }
    }
}
