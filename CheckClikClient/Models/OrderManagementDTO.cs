using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class OrderManagementDTO
    {
        public long OrderId { get; set; }
        public long OrderItemId { get; set; }
        public long UserId { get; set; }
        public string InvoiceNo { get; set; }
        public long BranchId { get; set; }
        public string BranchEn { get; set; }
        public string BranchAr { get; set; }
        public string Comments { get; set; }
        public int AddressId { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public int OrderType { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusEn { get; set; }

        public int StatusType { get; set; }
        public int Type { get; set; }
        public long Recipient { get; set; }
        public int StatusId { get; set; }
        public DateTime ExpectingDelivery { get; set; }
        public DateTime RescheduleDate { get; set; }
        public bool PaymentStatus { get; set; }
        public string TimeSlot { get; set; }
        public decimal SubTotal { get; set; }
        public decimal VAT { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public DateTime OrderDate { get; set; }
        public string jItems { get; set; }
        public string DeviceToken { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public string StatusMessage { get; set; }
        public string StatusCode { get; set; }
        public string dataxml { get; set; }
        public JArray ArrData { get; set; }
        public JObject JData { get; set; }
        public string DashboardCount { get; set; }
        public string Pending { get; set; }
        public string Scheduled { get; set; }
        public string ProcessOrders { get; set; }
        public bool IsCash { get; set; }
        public bool IsPayNow { get; set; }
        public bool IsPayLater { get; set; }
        public long TransferType { get; set; }
        public long TransferId { get; set; }
        public long TimeSlotId { get; set; }
    }
}