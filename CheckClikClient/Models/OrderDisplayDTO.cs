using CheckClickClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class OrderDisplayDTO
    {
        public Int64 Id { get; set; }
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public int Type { get; set; }
        public string InvoiceNo { get; set; }
        public long BranchId { get; set; }
        public string Comments { get; set; }
        public string BranchNameEn { get; set; }
        public string BranchNameAr { get; set; }
        public int AddressId { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public int OrderType { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusEn { get; set; }
        public string OrderStatusAr { get; set; }
        public int StatusType { get; set; }

        public int StatusId { get; set; }
        public int ItemsCount { get; set; }
        public DateTime ExpectingDelivery { get; set; }
        public bool PaymentStatus { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal VAT { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal ShipmentCODFee { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime RequestDate { get; set; }
        public string jItems { get; set; }
        public string DeviceToken { get; set; }
        public string StoreType { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public string StatusMessage { get; set; }
        public string StatusCode { get; set; }
        public string dataxml { get; set; }


        public List<OrderItemDTO> Items { get; set; }
        public List<CustomerRegistrationDTO> UserDetails { get; set; }
        public List<OrderTrackingDTO> TrackingDetails { get; set; }
        public List<CustomerRegistrationDTO> ShipperAddress { get; set; }

        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }

        public List<OrderServiceProviderDTO> ServiceProviders { get; set; }
        public List<OrderRecipientDTO> Recipients { get; set; }
        public int ReturnType { get; set; }
        public string CheckoutId { get; set; }
        public string awbNo { get; set; }
        public decimal VatPercentage { get; set; }
    }
}