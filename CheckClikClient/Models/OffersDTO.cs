
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customer.Models
{
    public class OffersDTO
    {
        public long Id { get; set; }
        public IEnumerable<OffersHomeBannersDTO> ListOfHomeBanners { get; set; }
        public IEnumerable<OffersCDTO> ListOffers { get; set; }
        public IEnumerable<OffersCDTO> ServiceListOffers { get; set; }
        public IEnumerable<OffersCDTO> ProductListOffers { get; set; }
        public List<BannersDTO> listBanners { get; set; }
        public List<StoreDTO> listStores { get; set; }
        public string ApiURL { get; set; }
        public string Latitude { get;  set; }
        public string Longitude { get;  set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string SubCategoryEn { get; set; }
        public string SubCategoryAr { get; set; }

        public OffersDTO()
        {
            //this.ApiURL = System.Configuration.ConfigurationManager.AppSettings["apiurl"].ToString();

        }
    }
    public class OffersHomeBannersDTO
    {
        public long Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public long OfferType { get; set; }
        public string BannerImageEn { get; set; }
        public string BannerImageAr { get; set; }
        public long Percentage { get; set; }
        public long ProductId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long CountryId { get; set; }
        public bool IsUptoApplicable { get; set; }
        public long WeekNo { get; set; }
        public long StoreId { get; set; }
    }

    public class OffersCDTO
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string IconClass { get; set; }
        public long OfferType { get; set; }
        public string BannerImageEn { get; set; }
        public string BannerImageAr { get; set; }
        public long Percentage { get; set; }
        public long ProductId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long CountryId { get; set; }
        public long StoreId { get; set; }
        public long IsVatApplicable { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double DiscountPrice { get; set; }

        public Int64 BranchId { get; set; }
        public Int64 BranchCategoryId { get; set; }
        public Int64 BranchSubCategoryId { get; set; }
        public Int64 BranchMainCategoryId { get; set; }
        public string SubCategoryImage { get; set; }
        public string SubCategoryNameEn { get; set; }
        public string SubCategoryNameAr { get; set; }
        public string MainCategoryImage { get; set; }
        public string MainCategoryNameEn { get; set; }
        public string MainCategoryNameAr { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string Image { get; set; }
        public string UPCBarcode { get; set; }
        public int DiscountType { get; set; }
        public Decimal DiscountValue { get; set; }
        public Decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public string ProductSkuId { get; set; }
        public long ServiceId { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
        public string CountingNameEn { get; set; }
        public string CountingNameAr { get; set; }
        public int SubCategoryId { get; set; }
        public string CategoryIds { get; set; }
        public bool Favorite { get; set; }
        public string StoreNameEn { get; set; }
        public string StoreNameAr { get; set; }
        public string GenerateSlug()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            //string phrase = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", InventoryId, ProductNameEn, ProductSkuId, idss, ProductId, UPCBarcode);

            //string str = RemoveAccent(phrase).ToLower();
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            string Brach = BranchId.ToString();
            string data = "";//String.Concat(StringUtil.URLEncrypt(NameEn.Replace(" ","-") + '_' + Brach + '_' + InventoryId + '_' + BranchSubCategoryId));
            //string phrase = string.Format("{0}-{1}-{2}-{3}", data, ProductSkuId, idss, ProductId, UPCBarcode);
            string phrase = string.Format("{0}", data);

            //string str = RemoveAccent(phrase).ToLower();
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //// convert multiple spaces into one space   
            //str = Regex.Replace(str, @"\s+", " ").Trim();
            //// cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //str = Regex.Replace(str, @"\s", "-"); // hyphens   
            string str = Regex.Replace(phrase, @"/", "-").Trim();
            return str;
        }

        public string GenerateSlugService()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            string Brach = BranchId.ToString();
            string data = "";//String.Concat(StringUtil.URLEncrypt(ServiceNameEn + '_' + Brach + '_' + ServiceId + '_' + BranchSubCategoryId));

            //string phrase = string.Format("{0}-{1}", data, CountingNameEn);
            string phrase = string.Format("{0}", data);

            //string str = RemoveAccent(phrase).ToLower();
            //// invalid chars           
            //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
            //// convert multiple spaces into one space   
            //str = Regex.Replace(str, @"\s+", " ").Trim();
            //// cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //str = Regex.Replace(str, @"\s", "-"); // hyphens   
            string str = Regex.Replace(phrase, @"/", "-").Trim();

            return str;
        }
        public string GenerateSlugItemList()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string idss = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            //string data = String.Concat(StringUtil.URLEncrypt(MainCategoryNameEn.Replace(" ", "-") + '_' + BranchId + '_' + BranchMainCategoryId  + '_'+ StoreId));
            string data = "";//String.Concat(StringUtil.URLEncrypt(NameEn.Replace(" ", "-") + '_' + BranchId + '_' + Id + '_' + StoreId));
            //string phrase = string.Format("{0}-{1}-{2}", ServiceId, ServiceNameEn,CountingNameEn);
            string phrase = string.Format("{0}", data);
            string str = phrase; //RemoveAccent(phrase).ToLower();
                                 // invalid chars           
                                 //str = Regex.Replace(str, @"[^a-z0-9\s-_]", "");
                                 // convert multiple spaces into one space   
                                 //str = Regex.Replace(str, @"\s+", " ").Trim();
             str = Regex.Replace(phrase, @"/", "-").Trim();
            // cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
           // str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

    }
}