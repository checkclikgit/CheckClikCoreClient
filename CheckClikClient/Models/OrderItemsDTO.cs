using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class OrderItemsDTO
    {
        public int Id { get; set; }
        public int ItemId { get; set; } = 0;
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int Qty { get; set; }
        public Decimal Price { get; set; }
        public Decimal VAT { get; set; }
        public Decimal GrandTotal { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string Image { get; set; }
        public string SKUId { get; set; }
        public Boolean IsAccepted { get; set; }    
        public string Reason { get; set; }

        public bool NotReturnable { get; set; }
        public int ReturnDays { get; set; }
        public int RemainingDays { get; set; }
        public int RemainingQnty { get; set; }
        public bool IsReturnRequest { get; set; }
        public int ReturnRequestStatus { get; set; }

        public long ReturnId { get; set; }
        public string invoice { get; set; }

        public IEnumerable<OrderItemsDTO> ReturnRequest { get; set; }
        public int StatusId { get; set; }
        public string StatusIdText { get; set; }
        public int ReturnType { get; set; }
        public string ReturnTypeText { get; set; }
        public string RequestDate { get; set; }
        public bool PaymentStatus { get; set; }
        public bool IsReturned { get; set; }
        public long Recipient { get; set; }
        public string Message { get; set; }
        public string ServiceProviderName { get; set; }
        public string CountingMeasureEn { get; set; }
        public string CountingMeasureAr { get; set; }



        public IEnumerable<OrderItemsDTO> ItemImage { get; set; }
    }
}
