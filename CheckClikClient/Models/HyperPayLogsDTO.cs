using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class HyperPayLogsDTO
    {
        public string TransactionId { get; set; }
        public Int64 UserId { get; set; }
        public string TransactionType { get; set; }
        public string RequestData { get; set; }
        public string CheckOutId { get; set; }
        public string TransactionDescription { get; set; }
        public int RequestType { get; set; }
        public int FlagId { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string JsonLog { get; set; }
        public long OrderId { get; set; }
        public string InvoiceNo { get; set; }
    }
}