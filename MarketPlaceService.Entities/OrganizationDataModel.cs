using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class OrganizationDataModel
    {
        public int? Organisationid { get; set; }
        public string Organisationname { get; set; }
        public int? ParentOrganisationid { get; set; }
        // public int Organisationtypeid { get; set; }
        // public string Organisationcode { get; set; }
        // public string Organisationdescription { get; set; }
    }
}
