using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class TransactionLog
    {
        public Guid TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionData { get; set; }
        public string InitiatedBy { get; set; }
        public DateTime InitiatedOn { get; set; }
        public Guid TraceId { get; set; }
    }
}
