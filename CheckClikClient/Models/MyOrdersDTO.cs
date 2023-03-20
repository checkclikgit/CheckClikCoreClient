using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class MyOrdersDTO
    {

        public int BranchId { get; set; }
        public string BranchLogoImage { get; set; }
        public Int64 Id { get; set; }
        public Int64 UserId { get; set; }
        public int Type { get; set; }
        public string InvoiceNo { get; set; }
        public IEnumerable<MyOrdersDTO> UsersPendingOrdersList { get; set; }
        public IEnumerable<MyOrdersDTO> UsersProcessingOrdersList { get; set; }
        public List<OrderItemsDTO> Items { get; set; }
        public List<OrderTrackingDTO> TrackingDetails { get; set; }
        public int ItemsCount { get; set; }
        public int RejectedItemsCount { get; set; }
        public Int64 OrderId { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusEn { get; set; }
        public string StoreNameEn { get; set; }
        public string StoreNameAr { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public int OrderType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Address { get; set; }
        public decimal AcceptedTotal { get; set; }
        public decimal AcceptedVAT { get; set; }
        public decimal AcceptedDeliveryFee { get; set; }
        public decimal AcceptedGrandTotal { get; set; }
        public string Message { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveredDate { get; set; } 
        public DateTime ExpectingDelivery { get; set; }
        public string Comments { get; set; }
        public DateTime OrderTrackingDate { get; set; }
        public DateTime ReScheduleDate { get; set; }
        public int StatusId { get; set; }
        public int Rating { get; set; }
        public string Ids { get; set; }
        //public int BranchId { get; set; }
        public string SKUId { get; set; }
        public string UrlPath { get; set; }
        public string ItemId { get; set; }
        public int SubCategoryId { get; set; }
        public bool PaymentStatus { get; set; }
        public string StatusMessage { get; set; }
        public string StatusCode { get; set; }
        public string awbNo { get; set; }
        public string reason { get; set; }
        public int PageNumber { get;  set; }
        public int pagingNumber { get; set; }
        public string TimeSlot { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal VAT { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal ShipmentCODFee { get; set; }
        public decimal AcceptedShipmentCODFee { get; set; }
        public decimal AcceptedDiscountAmount { get; set; }

        public string ApiURL { get; set; }
        public MyOrdersDTO()
        {
            //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();
        }

    }

    public class ReturnOrders_temp
    {
        public Int64 OrderId { get; set; }
        public long UserId { get; set; }
        public IEnumerable<ReturnOrders_temp> JJItems { get; set; }
        public long OrderItemId { get; set; }
        public int Qty { get; set; }
        public IEnumerable<ReturnOrders_temp> Images { get; set; }
        public string FileName { get; set; }
        public string Base64FileData { get; set; }

        public IEnumerable<ReturnOrders_temp> masterlst { get; set; }

        public string FullName { get; set; }
        public string AccountNo { get; set; }
        public string IbanNo { get; set; }
        public string BankName { get; set; }
        public string BankBIC { get; set; }

        public string JItems { get; set; }
        

    }
}