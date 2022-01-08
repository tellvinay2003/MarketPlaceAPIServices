using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class TransactionLogDataModel<T>
    {
        public Guid TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public T TransactionData { get; set; }
        public string InitiatedBy { get; set; }
        public DateTime InitiatedOn { get; set; }
        public Guid TraceId { get; set; }
    }
}
