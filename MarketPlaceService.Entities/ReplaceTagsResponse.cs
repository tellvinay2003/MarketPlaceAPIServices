using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class ReplaceTagsResponse
    {   
        public int DataTypeId { get; set; }
        public string DataTypeName { get; set; }
        public string TagName { get; set; }
        public string TagFullPath { get; set; }
        public bool IsMappingMandatory { get; set; }
        public List<MappingList> MappingValues { get; set; }   
        public string RemoveTag { get; set; }
    }
    public class MappingList
    {
        public int? SourceValueId { get; set; }
        public int? TagetValueId { get; set; }
        public string SourceValueName { get; set; }
        public string TagetValueName { get; set; }
       
    }
}
