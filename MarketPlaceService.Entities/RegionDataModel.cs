using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class RegionDataModel
    {
        public int REGIONID { get; set; }
        public string REGIONSHORTNAME { get; set; }
        public string REGIONNAME { get; set; }
        public string REGIONDESCRIPTION { get; set; }
        public int REGIONTYPEID { get; set; }
        public string REMOTEKEY { get; set; }
        public string LATITUDES { get; set; }
        public string LONGITUDES { get; set; }
        public Guid TIMEZONEID { get; set; }
        public string DONOTAPPLYCLIENTBAND { get; set; }
        public Guid LEVEL10_REGIONID { get; set; }
        public string FITPROPOSAL { get; set; }
        public string GROUPQUOTE { get; set; }
        public string RADIUS { get; set; }

    }
}
