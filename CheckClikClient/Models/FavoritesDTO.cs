using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customer.Models
{
    public class FavoritesDTO
    {
        public Int64 Id { get; set; }
        public long UserId { get; set; }
        public int Type { get; set; }
        public string Logo { get; set; }
        public string BranchLogoImage { get; set; }
        public string StoreEn { get; set; }
        public string StoreAr { get; set; }
        public int Reviews { get; set; }
        public int ReviewsCount { get; set; }
        public float Distance { get; set; }
        public string MinimumOrderValue { get; set; }
        public string DeliveryTime { get; set; }
        public string BranchStatus { get; set; }
        public bool IsCashAllowed { get; set; }
        public bool IsCreditCardAllowed { get; set; }
        public int VendorType { get; set; }
        public string ApiURL { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
        public Decimal DiscountValue { get; set; }
        public int DiscountType { get; set; }
        public Decimal SellingPrice { get; set; }
        public Decimal Price { get; set; }
        public int BranchId { get; set; }
        public int StoreId { get; set; }
        public int StoreType { get; set; }
        public IEnumerable<FavoritesDTO> StoreDetailslst { get; set; }
        public IEnumerable<FavoritesDTO> ProductDetailslst { get; set; }
        public IEnumerable<FavoritesDTO> ServiceDetailslst { get; set; }
        public string Image { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string ProductSkuId { get; set; }
        public string UPCBarcode { get; set; }
        public int ProductId { get; set; }
        public string ItemId { get; set; }
        public int FavoriteId { get; set; }
        public int MainCatId { get; set; }
        public string MainCatEn { get; set; }
        public int SubCatId { get; set; }
        public string SubCatEn { get; set; }
        public int ServiceId { get; set; }
        //public int StoreRowCount { get; set; }
        //public int ProductRowCount { get; set; }
        //public int ServiceRowCount { get; set; }

        public int StorepagingNumber { get; set; }
        public int ProductpagingNumber { get; set; }
        public int ServicepagingNumber { get; set; }

        public int StorePageNumber { get; set; }
        public int ProductPageNumber { get; set; }
        public int ServicePageNumber { get; set; }
        public FavoritesDTO()
        {
            //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();

        }
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

    }
}