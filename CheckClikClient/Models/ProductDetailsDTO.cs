using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customer.Models
{
    public class ProductDetailsDTO
    {
        public int CartQuantity { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public int StoreId { get; set; }
        public string SkuId { get; set; }
        public int BranchId { get; set; }
        public long ProductInventoryId { get; set; }
        public long ProductId { get; set; }
        public string ProductSkuId { get; set; }
        public int InventoryMethod { get; set; }
        public int StockQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public bool DisplayStockQuantity { get; set; }
        public int LowStockActivity { get; set; }
        public int ProductAvailabilityRange { get; set; }
        public int MinCartQty { get; set; }
        public int MaxCartQty { get; set; }
        public int AllowedQty { get; set; }
        public bool NotReturnable { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public bool IsVatApplicable { get; set; }
       
        public int OptionId { get; set; }
        public string VariantNameAr { get; set; }
        public string VariantNameEn { get; set; }

        public Decimal DiscountValue { get; set; }
        public Decimal Price { get; set; }
        public Decimal SellingPrice { get; set; }
        public string UPCBarcode { get; set; }
        public int DiscountType { get; set; }
        public int Condition { get; set; }
        public int ProductCountingNo { get; set; }

        public string ValueEn { get; set; }
        public string ValueAr { get; set; }
        public bool Favorite { get; set; }

        public IEnumerable<ProductDetailsDTO> ProductVariantslst { get; set; }
        public IEnumerable<ProductDetailsDTO> mastervariantslst { get; set; }

        public IEnumerable<ProductDetailsDTO> JProductVariant { get; set; }
        public IEnumerable<ProductDetailsDTO> Jvariants { get; set; }

        public IEnumerable<ProductDetailsDTO> ProductDetailsLst { get; set; }
        public IEnumerable<ProductDetailsDTO> VariantsLst { get; set; }

        public IEnumerable<ProductDetailsDTO> JReviews { get; set; }
        public IEnumerable<ProductDetailsDTO> JRelatedProducts { get; set; }
        public IEnumerable<ProductDetailsDTO> JRelatedServices { get; set; }

        public JObject Data { get; set; }
        public string UrlPath { get; set; }

        public string Image { get; set; }
        public long ImageId { get; set; }

        public string Images { get; set; }
        public IEnumerable<ProductDetailsDTO> JImages { get; set; }
        public IEnumerable<ProductDetailsDTO> JServiceProvidersJson { get; set; }
        
        public int loginval { get; set; }
        public int AvailableQuantity { get; set; }
        public int CartBranchId { get; set; }
        public int CartItems { get; set; }
        public int SubCategoryId { get; set; }
        public int ServiceId { get; set; }

        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
        public int ServiceCountingMeasure { get; set; }
        public string ServiceCountingNameEn { get; set; }
        public string ServiceCountingNameAr { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public long ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public string TimingType { get; set; }
        public string Disabledays { get; set; }

        public long TimeSlotId { get; set; }
        public string TimeSlot { get; set; }
      
        public int WeekdayId { get; set; }
        public string Weekday { get; set; }

        public int OrderType { get; set; }
       
        public DateTime RequestDate { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }

        public DateTime ReviewDate { get; set; }
        public string DisplayName { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public int RemainingRating { get; set; }
        public long Id { get; set; }


        public string GenerateSlug()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            string phrase = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", Id, ProductNameEn, ProductSkuId, idss, ProductId, UPCBarcode);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }
        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        public long PurchaseCount { get; set; }
        public int UserReviews { get; set; }
        public long OrderId { get; set; }

        public string GenerateSlugService()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            string phrase = string.Format("{0}-{1}-{2}", ServiceId, ServiceNameEn, CountingNameEn);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }
        public string CountingNameEn { get; set; }
        public string CountingNameAr { get; set; }
        public string USession { get; set; }
        public int MainCatId { get; set; }
        public string MainCatEn { get; set; }
        public string MainCatAr { get; set; }
        public int SubCatId { get; set; }
        public string SubCatEn { get; set; }
        public string SubCatAr { get; set; }
        public string StoreNameEn { get; set; }
        public string MetaDescriptionAr { get; set; }
        public string MetaDescriptionEn { get; set; }
        public int AvgUserReviews { get; set; }

    }
}
