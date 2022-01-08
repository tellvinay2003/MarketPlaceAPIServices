using System;

namespace MarketPlaceService.Entities
{
    public class MessageFieldDetails
    {
        public string FieldName { get; set; }
        public string FieldPath { get; set; }
        public int MappingDataType { get; set; }
        public bool IsMappingMandatory { get; set; }
        public string DataTypeName { get; set; }
        public string RemoveTag {get;set;}
    }
}
