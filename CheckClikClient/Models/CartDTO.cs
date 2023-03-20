using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class CartDTO
    {
        public Decimal TotalAmount { get; set; }
        public string PickupInServiceWeekdays { get; set; }
        public string DeliveryHomeWeekdays { get; set; }
        public JObject Data { get; set; }
        public string UrlPath { get; set; }
        public int BranchId { get; set; }
        public int StoreId { get; set; }
        public IEnumerable<CartDTO> CartList { get; set; }
        public long CouponId { get; set; }
        //public decimal MaxDiscountAmount { get; set; }

        public long CartId { get; set; }
        public long UserId { get; set; }
        public string ProductSkuId { get; set; }
        public DateTime Date { get; set; }
        public int CartQuantity { get; set; }
        public int StockQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public bool DisplayStockQuantity { get; set; }
        public int LowStockActivity { get; set; }
        public int ProductAvailabilityRange { get; set; }
        public int MinCartQty { get; set; }
        public int MaxCartQty { get; set; }
        public int AllowedQty { get; set; }
        public bool NotReturnable { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }
        public Decimal Price { get; set; }
        public Decimal TotalVatAmount { get; set; }
        public Decimal VatAmountPerItem { get; set; }
        public Decimal TotalItemPrice { get; set; }
        public Decimal CheckOutAmount { get; set; }
        public bool IsVatApplicable { get; set; }
        public string ProductNameAr { get; set; }
        public string ProductNameEn { get; set; }
        public int AvailableQuantity { get; set; }
        public int FlagId { get; set; }
        public string message { get; set; }
        public int WeekdayId { get; set; }
        public string Weekday { get; set; }

        public string PickUpInStoreType { get; set; }
        public int valfirst { get; set; }
        public string DeliveryHomeService { get; set; }
        public int valtwo { get; set; }
        public int PreBookingInStoreTimeUnit { get; set; }
        public int PreBookingDeliveryTimeUnit { get; set; }

        public string TimingType { get; set; }
        public int Acceptedorders { get; set; }
        public int TotalAcceptOrders { get; set; }
        public long SlotId { get; set; }
        public string TimeSlot { get; set; }
        public bool CartStatus { get; set; }
        public IEnumerable<CustomerManageAddressDTO> ListOfCustomerAddress { get; set; }
        public string Comments { get; set; }
        public long AddressId { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public int OrderType { get; set; }
        public DateTime ExpectingDelivery { get; set; }
        public bool PaymentStatus { get; set; }
        public long TimeSlotId { get; set; }

        public string ServiceType { get; set; }
        public string ServiceProviderName { get; set; }

        public IEnumerable<CartDTO> CouponList { get; set; }

        public IEnumerable<CartDTO> ProductList { get; set; }
        public int Id { get; set; }
        public string VoucherNameEn { get; set; }
        public string VoucherNameAr { get; set; }
        public int VoucherDiscount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public string CouponCode { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public string BaseUrl_c { get; set; }
        public string Address { get; set; }
        public string RequestDate { get; set; }
        public string USession { get; set; }
        public int CouponExpired { get; set; }
        public bool InStoreService { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsShippingWorldwide { get; set; }
        public bool ShippingProvider { get; set; }
        public decimal DelDeliveryFee { get; set; }
        public decimal ShippingDeliveryFee { get; set; }
        public decimal ShipmentCODFee { get; set; }
        public long CreatedBy { get; set; }
        public long CityId { get; set; }
        public bool AvailabilityStatus { get; set; }
        public DateTime BranchExpiry { get; set; }
        public int BranchExpiryDays { get; set; }
        public decimal MinimumOrderValue { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal VAT { get; set; }
        public decimal GrandTotal { get; set; }
    }
}