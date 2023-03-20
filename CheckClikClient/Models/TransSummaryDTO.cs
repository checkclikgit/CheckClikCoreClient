using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class TransSummaryDTO
    {
        public string Price { get; set; }
        public string InvoiceNo { get; set; }
        public string PayFor { get; set; }
        public string Msg { get; set; }
    }
}