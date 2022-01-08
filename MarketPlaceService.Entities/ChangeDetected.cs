using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class ChangeDetected
    {
        public string ChangeType { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
    }

    public enum Action
    {
        Added,
        Changed,
        Deleted
    }
}
