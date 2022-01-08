using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
   public class OrganizationPrefixDataModel
    {
        public int? Organisationid { get; set; }
        public string Organisationname { get; set; }
        public int? ParentOrganisationid { get; set; }

        public IEnumerable<BookingPrefixDataModel> BookingPrefix {get;set;}

    }


 
}
