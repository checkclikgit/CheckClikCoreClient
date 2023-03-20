using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int ItemId { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int Qty { get; set; }
        public int ReturnQty { get; set; }

        public Decimal Price { get; set; }
        public Decimal VAT { get; set; }
        public Decimal GrandTotal { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string Image { get; set; }
        public string ItemImage { get; set; }
        public string SKUId { get; set; }
        public Boolean IsAccepted { get; set; }
        public List<ItemVariantsDTO> Variants { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerAr { get; set; }
        public int ServiceProviderId { get; set; }
        public decimal Weight { get; set; }
        public string CountingMeasureEn { get; set; }
        public string CountingMeasureAr { get; set; }
        public string ServiceProviderName { get; set; }

    }
}
